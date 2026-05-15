using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using Lagrange.Milky.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.Milky.Event;

public class MilkyWebSocketEventService(ILogger<MilkyWebSocketEventService> logger, IOptions<MilkyConfiguration> options, EventService @event) : IHostedService
{
    private readonly ILogger<MilkyWebSocketEventService> _logger = logger;

    private readonly string _host = options.Value.Host ?? throw new Exception("Milky.Host cannot be null");
    private readonly ulong _port = options.Value.Port ?? throw new Exception("Milky.Port cannot be null");
    private readonly string _path = $"{options.Value.Prefix}{(options.Value.Prefix.EndsWith('/') ? "" : "/")}event";
    private readonly string? _token = options.Value.AccessToken;

    private readonly EventService _event = @event;

    private readonly HttpListener _listener = new();
    private readonly ConcurrentDictionary<ConnectionContext, object?> _connections = [];

    private Task? _task;
    private CancellationTokenSource? _cts;

    public Task StartAsync(CancellationToken ct)
    {
        _listener.Prefixes.Add($"http://{_host}:{_port}{_path}/");
        _listener.Start();

        foreach (string prefix in _listener.Prefixes) _logger.LogServerRunning(prefix);

        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _task = GetHttpContextLoopAsync(_cts.Token);

        _event.Register(HandleEventAsync);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken ct)
    {
        _event.Unregister(HandleEventAsync);

        _cts?.Cancel();
        if (_task != null) await _task.WaitAsync(ct).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        await Task.WhenAll(_connections.Keys.Select(connection => connection.Tcs.Task));

        _listener.Stop();
    }

    private async Task? GetHttpContextLoopAsync(CancellationToken ct)
    {
        try
        {
            while (true)
            {
                _ = HandleHttpContextAsync(await _listener.GetContextAsync().WaitAsync(ct), ct);

                ct.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception e)
        {
            _logger.LogGetHttpContextException(e);
            throw;
        }
    }

    private async Task HandleHttpContextAsync(HttpListenerContext context, CancellationToken ct)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;
        string method = request.HttpMethod;
        string? rawUrl = request.RawUrl;

        try
        {
            _logger.LogHttpContext(identifier, remote, method, rawUrl);

            if (!await ValidateHttpContextAsync(context, ct)) return;

            var connection = await GetConnectionContextAsync(context, ct);
            if (connection == null) return;

            _ = WaitConnectionCloseLoopAsync(connection, connection.Cts.Token);
        }
        catch (OperationCanceledException)
        {
            await SendWithLoggerAsync(context, HttpStatusCode.InternalServerError, ct);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogHandleHttpContextException(identifier, remote, e);
            await SendWithLoggerAsync(context, HttpStatusCode.InternalServerError, ct);
        }
    }

    private async Task WaitConnectionCloseLoopAsync(ConnectionContext connection, CancellationToken ct)
    {
        var identifier = connection.HttpContext.Request.RequestTraceIdentifier;
        var remote = connection.HttpContext.Request.RemoteEndPoint;

        try
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                var resultTask = connection.WsContext.WebSocket
                        .ReceiveAsync(buffer.AsMemory(), default);

                var result = !resultTask.IsCompleted ?
                    await resultTask.AsTask().WaitAsync(ct) :
                    resultTask.Result;

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseConnectionAsync(connection, WebSocketCloseStatus.NormalClosure, ct);
                    return;
                }

                ct.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException)
        {
            await CloseConnectionAsync(connection, WebSocketCloseStatus.NormalClosure, default);
        }
        catch (Exception e)
        {
            _logger.LogWaitWebSocketCloseException(identifier, remote, e);

            await CloseConnectionAsync(connection, WebSocketCloseStatus.InternalServerError, ct);
        }
    }

    private async Task CloseConnectionAsync(ConnectionContext connection, WebSocketCloseStatus status, CancellationToken ct)
    {
        var identifier = connection.HttpContext.Request.RequestTraceIdentifier;
        var remote = connection.HttpContext.Request.RemoteEndPoint;

        try
        {
            _connections.Remove(connection, out _);

            await connection.WsContext.WebSocket.CloseAsync(status, null, ct);
            connection.HttpContext.Response.Close();

            _logger.LogWebSocketClosed(identifier, remote);
        }
        catch (Exception e)
        {
            _logger.LogWebSocketCloseException(identifier, remote, e);
        }
        finally
        {
            connection.Tcs.SetResult();
        }
    }

    private async Task<bool> ValidateHttpContextAsync(HttpListenerContext context, CancellationToken ct)
    {

        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;

        if (request.Url?.LocalPath != _path)
        {
            await SendWithLoggerAsync(context, HttpStatusCode.NotFound, ct);
        }

        if (!context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            await SendWithLoggerAsync(context, HttpStatusCode.MethodNotAllowed, ct);
            return false;
        }

        if (!request.IsWebSocketRequest)
        {
            await SendWithLoggerAsync(context, HttpStatusCode.BadRequest, ct);
            return false;
        }

        if (!ValidateAccessToken(context))
        {
            _logger.LogValidateAccessTokenFailed(identifier, remote);
            await SendWithLoggerAsync(context, HttpStatusCode.Unauthorized, ct);
            return false;
        }

        return true;
    }

    private bool ValidateAccessToken(HttpListenerContext context)
    {
        if (string.IsNullOrEmpty(_token)) return true;

        string? authorization = context.Request.Headers["Authorization"];
        if (authorization != null)
        {
            if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) return false;
            return authorization.AsSpan(7..).Equals(_token);
        }

        string? accessToken = context.Request.QueryString["access_token"];
        if (accessToken != null)
        {
            return accessToken.Equals(_token);
        }

        return false;
    }

    private async Task<ConnectionContext?> GetConnectionContextAsync(HttpListenerContext context, CancellationToken ct)
    {
        try
        {
            var wsContext = await context.AcceptWebSocketAsync(null).WaitAsync(ct);
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var connection = new ConnectionContext(context, wsContext, cts);
            _connections.TryAdd(connection, null);
            return connection;
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception)
        {
            await SendWithLoggerAsync(context, HttpStatusCode.InternalServerError, ct);
            throw;
        }
    }

    private async Task SendWithLoggerAsync(HttpListenerContext context, HttpStatusCode status, CancellationToken ct)
    {
        var request = context.Request;
        var identifier = request.RequestTraceIdentifier;
        var remote = request.RemoteEndPoint;

        var response = context.Response;
        var output = response.OutputStream;

        try
        {
            int code = (int)status;

            response.StatusCode = code;
            await output.WriteAsync(Encoding.UTF8.GetBytes($"{code} {status}"), ct);
            response.Close();

            _logger.LogSend(identifier, remote, status);
        }
        catch (Exception e)
        {
            _logger.LogSendException(identifier, remote, e);
        }
    }

    private async void HandleEventAsync(Memory<byte> payload)
    {
        if (_connections.IsEmpty) return;

        _logger.LogSend(payload.Span);
        foreach (var connection in _connections.Keys)
        {
            var identifier = connection.HttpContext.Request.RequestTraceIdentifier;
            var remote = connection.HttpContext.Request.RemoteEndPoint;
            var ws = connection.WsContext.WebSocket;

            try
            {
                await connection.SendSemaphoreSlim.WaitAsync(connection.Cts.Token);
                try
                {
                    await ws.SendAsync(payload, WebSocketMessageType.Text, true, connection.Cts.Token);
                }
                finally
                {
                    connection.SendSemaphoreSlim.Release();
                }
            }
            catch (Exception e)
            {
                _logger.LogSendException(identifier, remote, e);

                await CloseConnectionAsync(connection, WebSocketCloseStatus.InternalServerError, connection.Cts.Token);
            }
        }
    }

    private class ConnectionContext(HttpListenerContext httpContext, WebSocketContext wsContext, CancellationTokenSource cts)
    {
        public HttpListenerContext HttpContext { get; } = httpContext;
        public WebSocketContext WsContext { get; } = wsContext;

        public SemaphoreSlim SendSemaphoreSlim { get; } = new(1);

        public CancellationTokenSource Cts { get; } = cts;
        public TaskCompletionSource Tcs { get; } = new();
    }
}

public static partial class MilkyWebSocketEventServiceLoggerExtension
{
    [LoggerMessage(LogLevel.Information, "Event websocket server is running on {prefix}")]
    public static partial void LogServerRunning(this ILogger<MilkyWebSocketEventService> logger, string prefix);

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} -->> {method} {path}")]
    public static partial void LogHttpContext(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote, string method, string? path);

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} <<-- {status}")]
    public static partial void LogSend(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote, HttpStatusCode status);

    [LoggerMessage(LogLevel.Debug, "WebSockets <<-- {payload}")]
    private static partial void LogSend(this ILogger<MilkyWebSocketEventService> logger, string payload);
    public static void LogSend(this ILogger<MilkyWebSocketEventService> logger, Span<byte> payload)
    {
        if (logger.IsEnabled(LogLevel.Debug)) logger.LogSend(Encoding.UTF8.GetString(payload));
    }

    [LoggerMessage(LogLevel.Debug, "{identifier} {remote} <//> WebSocket closed")]
    public static partial void LogWebSocketClosed(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote);


    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> WebSocket close failed")]
    public static partial void LogWebSocketCloseException(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote, Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Wait websocket close failed")]
    public static partial void LogWaitWebSocketCloseException(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote, Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Send failed")]
    public static partial void LogSendException(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote, Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Handle http context failed")]
    public static partial void LogHandleHttpContextException(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote, Exception e);

    [LoggerMessage(LogLevel.Error, "{identifier} {remote} <!!> Validate access token failed")]
    public static partial void LogValidateAccessTokenFailed(this ILogger<MilkyWebSocketEventService> logger, Guid identifier, IPEndPoint remote);

    [LoggerMessage(LogLevel.Error, "Get http context failed")]
    public static partial void LogGetHttpContextException(this ILogger<MilkyWebSocketEventService> logger, Exception e);
}