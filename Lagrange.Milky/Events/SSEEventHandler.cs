using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Events.Converters;
using Lagrange.Milky.Events.Extensions;
using Lagrange.Milky.Http;
using Lagrange.Milky.Models;
using Lagrange.Milky.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lagrange.Milky.Events;

public sealed class SSEEventHandler : IHttpHandler, IGenericEventHandler, IDisposable
{
    private static readonly byte[] HeartbeatLine = Encoding.UTF8.GetBytes(": heartbeat\n\n");
    private static readonly byte[] EventLine = Encoding.UTF8.GetBytes("event: milky_event\n");
    private static readonly byte[] DataLinePrefix = Encoding.UTF8.GetBytes("data: ");
    private static readonly byte[] DataLineSuffix = Encoding.UTF8.GetBytes("\n\n");

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SSEEventHandler> _logger;

    private readonly BotContext _lagrange;

    private readonly string? _token;
    private readonly string? _allowCorsOrigins;
    private readonly ulong _heartbeatIntervalSeconds;

    private readonly ConcurrentDictionary<Guid, (HttpListenerResponse Response, SemaphoreSlim SendLock)> _connections = new();
    private readonly CancellationTokenSource _cts = new();

    private int _disposed = 0;

    public SSEEventHandler(IServiceScopeFactory scopeFactory, ILogger<SSEEventHandler> logger, MilkyConfiguration configuration, MilkySSEEventConfiguration sseConfiguration, BotContext lagrange)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        _lagrange = lagrange;

        _token = configuration.AccessToken;
        _allowCorsOrigins = sseConfiguration.AllowCorsOrigins;
        _heartbeatIntervalSeconds = sseConfiguration.HeartbeatIntervalSeconds;

        _lagrange.RegisterConvertibleEvents(this);
    }

    public bool CanHandle(HttpListenerContext context)
        => context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)
        && context.Request.Url != null
        && context.Request.Url.AbsolutePath.Equals("/event");

    public async Task HandleAsync(HttpListenerContext context, CancellationToken ct)
    {
        var id = context.Request.RequestTraceIdentifier;

        try
        {
            if (!ValidateAccessToken(context))
            {
                _logger.LogAccessTokenRejected(id);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.Close();
                return;
            }

            context.Response.ContentType = MediaTypeNames.Text.EventStream;
            context.Response.Headers["Cache-Control"] = "no-cache";
            context.Response.Headers["Connection"] = "keep-alive";
            if (_allowCorsOrigins != null)
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = _allowCorsOrigins;
            }
            await context.Response.OutputStream.FlushAsync(ct);

            var @lock = new SemaphoreSlim(1);
            _connections.TryAdd(id, (context.Response, @lock));

            _logger.LogConnectionEstablished(id);

            await KeepAliveLoop(id, context, @lock, ct);
        }
        catch (Exception e)
        {
            _logger.LogConnectionError(id, e);
            context.Response.Abort();
        }
    }

    private async Task KeepAliveLoop(Guid id, HttpListenerContext context, SemaphoreSlim @lock, CancellationToken ct)
    {
        try
        {
            PeriodicTimer timer = new(TimeSpan.FromSeconds(_heartbeatIntervalSeconds));
            while (await timer.WaitForNextTickAsync(ct))
            {
                await @lock.WaitAsync(ct);
                try
                {
                    await context.Response.OutputStream.WriteAsync(HeartbeatLine, ct);
                    await context.Response.OutputStream.FlushAsync(ct);
                }
                finally
                {
                    @lock.Release();
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogClosedByServerShutdown(id);

            if (_connections.Remove(id, out var connectContext))
            {
                connectContext.SendLock.Dispose();
                connectContext.Response.Close();
            }
        }
        catch (Exception e)
        {
            _logger.LogKeepAliveError(id, e);
        }
    }

    private bool ValidateAccessToken(HttpListenerContext context)
    {
        return string.IsNullOrEmpty(_token) || ValidateAuthorization() || ValidateQueryString();

        bool ValidateAuthorization()
        {
            string? authorization = context.Request.Headers["Authorization"];
            return !string.IsNullOrEmpty(authorization)
                && authorization.StartsWith("Bearer ", StringComparison.Ordinal)
                && authorization.AsSpan(7).Equals(_token, StringComparison.Ordinal);
        }

        bool ValidateQueryString()
        {
            string? queryToken = context.Request.QueryString["access_token"];
            return !string.IsNullOrEmpty(queryToken) && queryToken.Equals(_token, StringComparison.Ordinal);
        }
    }

    private async Task SendAllAsync(byte[] payload, CancellationToken ct = default)
    {
        if (_connections.IsEmpty) return;

        foreach (var kvp in _connections)
        {
            var (response, @lock) = kvp.Value;

            await @lock.WaitAsync(ct);
            try
            {
                await response.OutputStream.WriteAsync(EventLine, ct);
                await response.OutputStream.WriteAsync(DataLinePrefix, ct);
                await response.OutputStream.WriteAsync(payload, ct);
                await response.OutputStream.WriteAsync(DataLineSuffix, ct);
                await response.OutputStream.FlushAsync(ct);
            }
            catch (Exception e)
            {
                _logger.LogSendError(kvp.Key, e);
            }
            finally
            {
                @lock.Release();
            }
        }
    }

    public async Task OnEvent<TEvent>(BotContext lagrange, TEvent @event) where TEvent : EventBase
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var converter = scope.ServiceProvider.GetRequiredService<IEventConverter<TEvent>>();
        byte[] bytes = Serializer.JsonSerializeToUtf8Bytes(new MilkyEvent
        {
            EventType = converter.Name,
            Time = DateTimeOffset.Now.ToUnixTimeSeconds(),
            SelfId = _lagrange.BotUin,
            Data = await converter.ConvertAsync(@event, _cts.Token)
        });

        await SendAllAsync(bytes, _cts.Token);
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 1) return;

        _lagrange.UnregisterConvertibleEvents(this);
    }
}

public static partial class SSEEventHandlerLoggerExtension
{
    [LoggerMessage(LogLevel.Warning, "{Id} rejected: invalid access token")]
    public static partial void LogAccessTokenRejected(this ILogger<SSEEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Information, "{Id} established")]
    public static partial void LogConnectionEstablished(this ILogger<SSEEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Error, "{Id} error occurred")]
    public static partial void LogConnectionError(this ILogger<SSEEventHandler> logger, Guid id, Exception ex);

    [LoggerMessage(LogLevel.Debug, "{Id} closed by client")]
    public static partial void LogClosedByClient(this ILogger<SSEEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Information, "{Id} closed due to server shutdown")]
    public static partial void LogClosedByServerShutdown(this ILogger<SSEEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Warning, "{Id} keep-alive loop error")]
    public static partial void LogKeepAliveError(this ILogger<SSEEventHandler> logger, Guid id, Exception ex);

    [LoggerMessage(LogLevel.Error, "{Id} send error")]
    public static partial void LogSendError(this ILogger<SSEEventHandler> logger, Guid id, Exception ex);
}