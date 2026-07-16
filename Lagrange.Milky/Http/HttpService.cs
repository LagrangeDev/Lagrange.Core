using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lagrange.Milky.Http;

public sealed class HttpService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _sf;

    private readonly ILogger<HttpService> _logger;

    private readonly HttpListener _listener;

    private Task? _getContextLoopTask;
    private readonly TaskCompletionTracker _handlerTaskTracker;
    private CancellationTokenSource? _cts;

    private int _disposed = 0;

    public HttpService(IServiceScopeFactory sf, ILogger<HttpService> logger, MilkyConfiguration configuration)
    {
        _sf = sf;
        _logger = logger;

        _listener = new HttpListener
        {
            Prefixes =
            {
                $"http://{configuration.HttpServer.Host}:{configuration.HttpServer.Port}/"
            }
        };

        _handlerTaskTracker = new();
    }

    public Task StartAsync(CancellationToken ct)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) == 1, this);

        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

        _listener.Start();
        _getContextLoopTask = GetContextLoop(_cts.Token);

        foreach (string prefix in _listener.Prefixes) _logger.LogStarted(prefix);

        return Task.CompletedTask;
    }

    private async Task GetContextLoop(CancellationToken ct)
    {
        try
        {
            while (true)
            {
                _handlerTaskTracker.Track(HandleContextAsync(await _listener.GetContextAsync().WaitAsync(ct), ct));
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            _logger.LogListenerLoopException(e);
        }
    }

    private async Task HandleContextAsync(HttpListenerContext context, CancellationToken ct)
    {
        var id = context.Request.RequestTraceIdentifier;
        var remoteIp = context.Request.RemoteEndPoint.Address;

        _logger.LogRequestReceived(id, remoteIp, context.Request.HttpMethod, context.Request.RawUrl ?? "");

        try
        {
            bool handled = false;
            await using var scope = _sf.CreateAsyncScope();
            foreach (var handler in scope.ServiceProvider.GetServices<IHttpHandler>())
            {
                if (!handler.CanHandle(context)) continue;

                _logger.LogRequestHandling(id, handler.GetType().Name);
                await handler.HandleAsync(context, ct);
                _logger.LogRequestCompleted(id, handler.GetType().Name);

                handled = true;
                break;
            }

            if (!handled)
            {
                _logger.LogRequestNotFound(id, context.Request.HttpMethod, context.Request.RawUrl ?? "");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.Close();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogRequestCancelled(id);
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            context.Response.Close();
        }
        catch (Exception e) // Unhandled exception
        {
            _logger.LogRequestUnhandledException(id, e);
            context.Response.Abort();
        }
    }

    public async Task StopAsync(CancellationToken ct)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) == 1, this);

        try { _cts?.Cancel(); } catch { }

        if (_getContextLoopTask != null)
        {
            await _getContextLoopTask.WaitAsync(ct).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }

        await _handlerTaskTracker.WaitAllAsync(ct).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        _listener.Stop();

        _logger.LogStopped();
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 1) return;

        _listener.Close();
    }
}

public static partial class HttpServiceLoggerExtension
{
    [LoggerMessage(LogLevel.Information, "HTTP service started on {Prefix}")]
    public static partial void LogStarted(this ILogger<HttpService> logger, string prefix);

    [LoggerMessage(LogLevel.Information, "HTTP service stopped")]
    public static partial void LogStopped(this ILogger<HttpService> logger);

    [LoggerMessage(LogLevel.Information, "Request {Id} received from {RemoteIP}: {Method} {Url}")]
    public static partial void LogRequestReceived(this ILogger<HttpService> logger, Guid id, IPAddress remoteIp, string method, string url);

    [LoggerMessage(LogLevel.Information, "Request {Id} is being handled by {HandlerType}")]
    public static partial void LogRequestHandling(this ILogger<HttpService> logger, Guid id, string handlerType);

    [LoggerMessage(LogLevel.Information, "Request {Id} processing completed by {HandlerType}")]
    public static partial void LogRequestCompleted(this ILogger<HttpService> logger, Guid id, string handlerType);

    [LoggerMessage(LogLevel.Warning, "Request {Id} not found: {Method} {Url}")]
    public static partial void LogRequestNotFound(this ILogger<HttpService> logger, Guid id, string method, string url);

    [LoggerMessage(LogLevel.Debug, "Request {Id} cancelled by server shutdown")]
    public static partial void LogRequestCancelled(this ILogger<HttpService> logger, Guid id);

    [LoggerMessage(LogLevel.Error, "Request {Id} unhandled exception")]
    public static partial void LogRequestUnhandledException(this ILogger<HttpService> logger, Guid id, Exception exception);

    [LoggerMessage(LogLevel.Error, "HTTP listener loop exception")]
    public static partial void LogListenerLoopException(this ILogger<HttpService> logger, Exception exception);
}