using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Meta;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public partial class ForwardWSService
{
    private ILogger _logger { get; }
    private ForwardWSServiceOptions _options { get; }
    private BotContext _context { get; }

    private HttpListener _listener { get; } = new();
    private ConcurrentDictionary<string, WebSocketConnectionContext> _connections { get; } = [];

    public ForwardWSService(ILogger<ForwardWSService> logger, IOptionsSnapshot<ForwardWSServiceOptions> options, BotContext context)
    {
        _logger = logger;
        _options = options.Value;
        _context = context;

        string host = _options.Host;
        if (host == "0.0.0.0") host = "*";

        _listener.Prefixes.Add($"http://{host}:{_options.Port}/");
    }
}

public partial class ForwardWSService : BackgroundService
{
    public override Task StartAsync(CancellationToken token)
    {
        _listener.Start();
        foreach (string prefix in _listener.Prefixes)
        {
            Log.LogServerStarted(_logger, prefix);
        }

        return base.StartAsync(token);
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            while (true)
            {
                HttpListenerContext httpContext = await _listener.GetContextAsync().WaitAsync(token);

                string identifier = Guid.NewGuid().ToString();

                Log.LogConnected(_logger, identifier);
                _ = HandleHttpContext(identifier, httpContext, token);
            }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Log.LogUnprocessedError(_logger, e);
        }
    }

    public override async Task StopAsync(CancellationToken token)
    {
        await base.StopAsync(token);

        await Task.WhenAll(_connections.Select(c => c.Value.Tcs.Task)).WaitAsync(token);

        _listener.Stop();
    }
}

public partial class ForwardWSService // Handler
{
    private async Task HandleHttpContext(string identifier, HttpListenerContext context, CancellationToken token) // no await, need to catch
    {
        HttpListenerResponse response = context.Response;

        try
        {
            if (!string.IsNullOrEmpty(_options.AccessToken) && !ValidationToken(context))
            {
                Log.LogAuthenticationFailed(_logger, identifier);

                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Close();

                return;
            }

            if (!context.Request.IsWebSocketRequest)
            {
                Log.LogNotWebSocket(_logger, identifier);

                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                response.Close();

                return;
            }

            HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null).WaitAsync(token);
            _ = HandleWebSocket(identifier, webSocketContext, token);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            try
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Close();
            }
            catch (Exception ex) { Log.LogHttpError(_logger, identifier, ex); }

            Log.LogHttpError(_logger, identifier, e);
        }
    }

    private async Task HandleWebSocket(string identifier, WebSocketContext context, CancellationToken token) // no await, need to catch
    {
        Task? receiveTask = null;
        Task? heartbeatTask = null;

        TaskCompletionSource tcs = new();

        try
        {
            _connections.TryAdd(identifier, new(context, tcs));

            CancellationTokenSource cts = new();

            string path = context.RequestUri.LocalPath;
            bool isApi = path == "/api" || path == "/api/";
            bool isEvent = path == "/event" || path == "/event/";

            if (!isApi)
            {
                await SendJsonAsync(new OneBotLifecycle(_context.BotUin, "connect"), identifier, token);
                heartbeatTask = HeartbeatLoop(identifier, cts.Token);
            }


            if (isEvent) receiveTask = WaitCloseLoop(identifier, token);
            else receiveTask = ReceiveLoop(identifier, token);

            try { await receiveTask; }
            finally { cts.Cancel(); }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Log.LogWebSocketError(_logger, identifier, e);
        }
        finally
        {
            receiveTask?.Dispose();

            if (heartbeatTask != null)
            {
                try { await heartbeatTask; }
                catch (OperationCanceledException) { }
                catch (Exception e) { Log.LogWebSocketError(_logger, identifier, e); }
            }
            heartbeatTask?.Dispose();

            try
            {
                await context.WebSocket
                    .CloseAsync(WebSocketCloseStatus.NormalClosure, null, default)
                    .WaitAsync(TimeSpan.FromSeconds(5), (CancellationToken)default);
            }
            catch (Exception e) { Log.LogWebSocketError(_logger, identifier, e); }
            context.WebSocket.Dispose();

            _connections.TryRemove(identifier, out _);

            tcs.SetResult();

            Log.LogDisconnected(_logger, identifier);
        }
    }

    private async Task ReceiveLoop(string identifier, CancellationToken token)
    {
        WebSocket ws = _connections[identifier].WsContext.WebSocket;

        byte[] buffer = new byte[1024];
        while (true)
        {
            int received = 0;
            while (true)
            {
                WebSocketReceiveResult result = await ws.ReceiveAsync(
                    new ArraySegment<byte>(buffer, received, buffer.Length - received),
                    default
                ).WaitAsync(token);
                if (result.MessageType == WebSocketMessageType.Close) return;
                received += result.Count;
                if (result.EndOfMessage) break;
                if (received == buffer.Length) Array.Resize(ref buffer, buffer.Length << 1);

                token.ThrowIfCancellationRequested();
            }
            string message = Encoding.UTF8.GetString(buffer, 0, received);
            Log.LogReceived(_logger, identifier, message);
            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(message, identifier));

            token.ThrowIfCancellationRequested();
        }
    }

    private async Task WaitCloseLoop(string identifier, CancellationToken token)
    {
        WebSocket webSocket = _connections[identifier].WsContext.WebSocket;

        byte[] buffer = new byte[1024];
        while (true)
        {
            if ((await webSocket.ReceiveAsync(buffer, default).WaitAsync(token)).MessageType == WebSocketMessageType.Close) return;

            token.ThrowIfCancellationRequested();
        }
    }

    private async Task HeartbeatLoop(string identifier, CancellationToken token)
    {
        uint heartBeatInterval = _options.HeartBeatInterval;
        TimeSpan interval = TimeSpan.FromMilliseconds(heartBeatInterval);
        while (true)
        {
            await SendJsonAsync(
                new OneBotHeartBeat(_context.BotUin, (int)heartBeatInterval, new OneBotStatus(true, true)),
                identifier, token
            );
            await Task.Delay(interval, token);

            token.ThrowIfCancellationRequested();
        }
    }

    private bool ValidationToken(HttpListenerContext context)
    {
        string? token = null;
        string? authorization = context.Request.Headers["Authorization"];
        if (authorization == null) token = context.Request.QueryString["access_token"];
        else if (authorization.StartsWith("Bearer ")) token = authorization["Bearer ".Length..];

        if (token != _options.AccessToken) return false;

        return true;
    }
}

public partial class ForwardWSService : ILagrangeWebService
{
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    public ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken token = default)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(json);
        if (identifier != null) return SendBytesAsync(identifier, bytes, token);
        else
        {
            ParallelQuery<Task> tasks = _connections
                .AsParallel()
                .Where(c => c.Value.WsContext.RequestUri.LocalPath is not "/api" and not "/api/")
                .Select(c => SendBytesAsync(c.Key, bytes, token).AsTask());
            return new(Task.WhenAll(tasks));
        }
    }

    public ValueTask SendBytesAsync(string identifier, byte[] bytes, CancellationToken token = default)
    {
        Log.LogSend(_logger, identifier, bytes);
        WebSocketConnectionContext connectionContext = _connections[identifier];
        lock (connectionContext)
        {
            return connectionContext.WsContext.WebSocket.SendAsync(bytes.AsMemory(), WebSocketMessageType.Text, true, token);
        }
    }
}

public partial class ForwardWSService // ConnectionContext class
{
    private partial class WebSocketConnectionContext(WebSocketContext wsContext, TaskCompletionSource tcs)
    {
        public WebSocketContext WsContext { get; set; } = wsContext;

        public TaskCompletionSource Tcs { get; set; } = tcs;
    }
}

public partial class ForwardWSService // Log class
{
    private static partial class Log
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Server started at {url}")]
        public static partial void LogServerStarted(ILogger logger, string url);

        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Connected(Conn: {identifier})")]
        public static partial void LogConnected(ILogger logger, string identifier);

        public static void LogSend(ILogger logger, string identifier, byte[] payload)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                string text = Encoding.UTF8.GetString(payload);

                if (text.Length > 1024)
                {
                    text = string.Concat(text.AsSpan(0, 1024), "...", (text.Length - 1024).ToString(), "bytes");
                }
                InternalLogSend(logger, identifier, text);
            }
        }

        [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = "Send(Conn: {identifier}): {s}", SkipEnabledCheck = true)]
        private static partial void InternalLogSend(ILogger logger, string identifier, string s);

        public static void LogReceived(ILogger logger, string identifier, string payload)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                if (payload.Length > 1024)
                {
                    payload = string.Concat(payload.AsSpan(0, 1024), "...", (payload.Length - 1024).ToString(), "bytes");
                }
                InternalLogReceived(logger, identifier, payload);
            }
        }

        [LoggerMessage(EventId = 3, Level = LogLevel.Trace, Message = "Receive(Conn: {identifier}): {s}", SkipEnabledCheck = true)]
        private static partial void InternalLogReceived(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Disconnected(Conn: {identifier})")]
        public static partial void LogDisconnected(ILogger logger, string identifier);


        [LoggerMessage(EventId = 995, Level = LogLevel.Warning, Message = "WebSocketError(Conn: {identifier})")]
        public static partial void LogWebSocketError(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 996, Level = LogLevel.Error, Message = "NotWebSocket(Conn: {identifier})")]
        public static partial void LogNotWebSocket(ILogger logger, string identifier);

        [LoggerMessage(EventId = 997, Level = LogLevel.Error, Message = "AuthenticationFailed(Conn: {identifier})")]
        public static partial void LogAuthenticationFailed(ILogger logger, string identifier);

        [LoggerMessage(EventId = 998, Level = LogLevel.Error, Message = "HttpError(Conn: {identifier})")]
        public static partial void LogHttpError(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 999, Level = LogLevel.Error, Message = "UnprocessedError")]
        public static partial void LogUnprocessedError(ILogger logger, Exception e);
    }
}
