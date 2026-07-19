using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Events.Extensions;
using Lagrange.Milky.Events.Converters;
using Lagrange.Milky.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Lagrange.Milky.Serialization;

namespace Lagrange.Milky.Events;

public sealed class WebSocketEventHandler : IHttpHandler, IGenericEventHandler, IDisposable
{
    private static readonly byte[] Buffer = new byte[1024];

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WebSocketEventHandler> _logger;

    private readonly BotContext _lagrange;

    private readonly string? _token;

    private readonly ConcurrentDictionary<Guid, (WebSocket WS, SemaphoreSlim SendLock)> _wss = new();
    private readonly CancellationTokenSource _cts = new();

    private int _disposed = 0;

    public WebSocketEventHandler(IServiceScopeFactory scopeFactory, ILogger<WebSocketEventHandler> logger, MilkyConfiguration configuration, MilkyWebSocketEventConfiguration wsConfiguration, BotContext lagrange)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        _lagrange = lagrange;

        _token = configuration.AccessToken;

        _lagrange.RegisterConvertibleEvents(this);
    }

    public bool CanHandle(HttpListenerContext context)
        => context.Request.Url != null
        && context.Request.Url.AbsolutePath.Equals("/event")
        && context.Request.IsWebSocketRequest;

    public async Task HandleAsync(HttpListenerContext httpContext, CancellationToken ct)
    {
        var id = httpContext.Request.RequestTraceIdentifier;

        try
        {
            if (!ValidateAccessToken(httpContext))
            {
                _logger.LogAccessTokenRejected(id);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.Close();
                return;
            }

            var wsContext = await httpContext.AcceptWebSocketAsync(null);
            _wss.TryAdd(id, (wsContext.WebSocket, new SemaphoreSlim(1)));

            _logger.LogConnectionEstablished(id);

            await KeepAliveLoop(id, wsContext.WebSocket, ct);
        }
        catch (Exception e)
        {
            _logger.LogConnectionError(id, e);
            httpContext.Response.Abort();
        }
    }

    private async Task KeepAliveLoop(Guid id, WebSocket ws, CancellationToken ct)
    {
        try
        {
            while (true)
            {
                ct.ThrowIfCancellationRequested();

                // We don't care what we receive, so let the buffer be overwritten as it may.
                var result = await ws.ReceiveAsync(Buffer, CancellationToken.None).WaitAsync(ct);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogClosedByClient(id);

                    if (_wss.Remove(id, out var connectContext)) connectContext.SendLock.Dispose();
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    ws.Dispose();
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogClosedByServerShutdown(id);

            if (_wss.Remove(id, out var connectContext)) connectContext.SendLock.Dispose();
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            ws.Dispose();
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
        if (_wss.IsEmpty) return;

        foreach (var kvp in _wss)
        {
            var (ws, @lock) = kvp.Value;

            await @lock.WaitAsync(ct);
            try
            {
                await ws.SendAsync(payload, WebSocketMessageType.Text, true, ct);
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

public static partial class WebSocketEventHandlerLoggerExtension
{
    [LoggerMessage(LogLevel.Warning, "{Id} rejected: invalid access token")]
    public static partial void LogAccessTokenRejected(this ILogger<WebSocketEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Information, "{Id} established")]
    public static partial void LogConnectionEstablished(this ILogger<WebSocketEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Error, "{Id} error occurred")]
    public static partial void LogConnectionError(this ILogger<WebSocketEventHandler> logger, Guid id, Exception ex);

    [LoggerMessage(LogLevel.Debug, "{Id} closed by client")]
    public static partial void LogClosedByClient(this ILogger<WebSocketEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Information, "{Id} closed due to server shutdown")]
    public static partial void LogClosedByServerShutdown(this ILogger<WebSocketEventHandler> logger, Guid id);

    [LoggerMessage(LogLevel.Warning, "{Id} keep-alive loop error")]
    public static partial void LogKeepAliveError(this ILogger<WebSocketEventHandler> logger, Guid id, Exception ex);

    [LoggerMessage(LogLevel.Error, "{Id} send error")]
    public static partial void LogSendError(this ILogger<WebSocketEventHandler> logger, Guid id, Exception ex);
}