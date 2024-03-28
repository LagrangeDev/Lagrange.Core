using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
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
    private ConcurrentDictionary<string, ForwardWSServiceWebSocketContext> _connections = [];

    public ForwardWSService(ILogger<ForwardWSService> logger, IOptionsSnapshot<ForwardWSServiceOptions> options, BotContext context)
    {
        _logger = logger;
        _options = options.Value;
        _context = context;

        _listener.Prefixes.Add($"http://{_options.Host}:{_options.Port}/");
    }
}

public partial class ForwardWSService
{
    private partial class ForwardWSServiceWebSocketContext(WebSocketContext wsContext)
    {
        public WebSocketContext WsContext { get; } = wsContext;
        public Task? ConnectionTask { get; set; }
    }

    private partial class ForwardWSServiceWebSocketContext : IDisposable
    {
        public void Dispose()
        {
            WsContext.WebSocket.Dispose();
            ConnectionTask?.Dispose();
        }
    }
}

public partial class ForwardWSService : BackgroundService, ILagrangeWebService
{
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    public override Task StartAsync(CancellationToken token)
    {
        if (IsPortInUse(_options.Port))
        {
            Log.LogPortInUse(_logger, _options.Port);
            return Task.CompletedTask;
        }

        _listener.Start();

        return base.StartAsync(token);
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (true)
        {
            _ = HandleHttp(await _listener.GetContextAsync().WaitAsync(token), token);
            token.ThrowIfCancellationRequested();
        }
    }

    public override async Task StopAsync(CancellationToken token)
    {
        await base.StopAsync(token);
        await Task.WhenAll(
            _connections
                .Where(c => c.Value.ConnectionTask != null)
                .Select(c => c.Value.ConnectionTask!)
        ).WaitAsync(token);
        _listener.Stop();
        return;
    }

    public async Task HandleHttp(HttpListenerContext httpContext, CancellationToken token)
    {
        string identifier = Guid.NewGuid().ToString();
        try
        {
            if (httpContext.Request.IsWebSocketRequest)
            {
                if (!string.IsNullOrEmpty(_options.AccessToken))
                {
                    string? accessToken = null;

                    string? authorization = httpContext.Request.Headers["Authorization"];
                    if (authorization == null)
                    {
                        accessToken = httpContext.Request.QueryString["access_token"];
                    }
                    else if (authorization.StartsWith("Bearer "))
                    {
                        accessToken = authorization[7..];
                    }

                    if (string.IsNullOrEmpty(accessToken))
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        httpContext.Response.AddHeader("WWW-Authenticate", "Bearer");
                        httpContext.Response.Close();
                        Log.LogAuthFailed(_logger, identifier);
                        return;
                    }

                    if (accessToken != _options.AccessToken)
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        httpContext.Response.Close();
                        Log.LogAuthFailed(_logger, identifier);
                        return;
                    }
                }

                HttpListenerWebSocketContext wsContext = await httpContext.AcceptWebSocketAsync(null).WaitAsync(token);

                ForwardWSServiceWebSocketContext connectionContext = new(wsContext);
                _connections[identifier] = connectionContext;
                connectionContext.ConnectionTask = HandleWebSocket(identifier, token);

                Log.LogConnected(_logger, identifier);
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                httpContext.Response.Close();
                return;
            }
        }
        catch (Exception e)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.Close();
            Log.LogErrorDisconnected(_logger, identifier, e);
        }
    }

    public async Task HandleWebSocket(string identifier, CancellationToken token)
    {
        WebSocketContext wsContext = _connections[identifier].WsContext;
        try
        {
            string uri = wsContext.RequestUri.LocalPath;
            if (uri != "/api" && uri != "/api/")
            {
                await SendJsonAsync(new OneBotLifecycle(_context.BotUin, "connect"), identifier, token);
            }

            CancellationTokenSource stopCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            Task[] tasks = wsContext.RequestUri.LocalPath switch
            {
                "/api" or "/api/" => [ReceiveLoop(identifier, stopCts.Token)],
                "/event" or "/event/" => [WaitClose(identifier, stopCts.Token), HeartbeatLoop(identifier, stopCts.Token)],
                _ => [ReceiveLoop(identifier, stopCts.Token), HeartbeatLoop(identifier, stopCts.Token)],
            };

            await Task.WhenAny(tasks);
            stopCts.Cancel();
            try { await Task.WhenAll(tasks); } catch (OperationCanceledException) { /* ignore */ }

            await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, token);
            Log.LogDisconnected(_logger, identifier);
        }
        catch (Exception e)
        {
            Log.LogErrorDisconnected(_logger, identifier, e);
        }
        finally
        {
            if (_connections.TryRemove(identifier, out ForwardWSServiceWebSocketContext? connectionContext))
            {
                connectionContext?.Dispose();
            }
        }
    }

    private async Task WaitClose(string identifier, CancellationToken token)
    {
        WebSocket ws = _connections[identifier].WsContext.WebSocket;
        byte[] buffer = [];
        while (true)
        {
            if ((await ws.ReceiveAsync(buffer, token)).CloseStatus != null) return;
            token.ThrowIfCancellationRequested();
        }
    }

    private async Task ReceiveLoop(string identifier, CancellationToken token)
    {
        WebSocket ws = _connections[identifier].WsContext.WebSocket;

        while (true)
        {
            byte[] buffer = new byte[1024];
            int received = 0;
            while (true)
            {
                ValueWebSocketReceiveResult result = await ws.ReceiveAsync(buffer.AsMemory(received), token);
                if (result.MessageType == WebSocketMessageType.Close) return;

                received += result.Count;
                if (result.EndOfMessage) break;

                if (received == buffer.Length) Array.Resize(ref buffer, received << 1);

                token.ThrowIfCancellationRequested();
            }

            string text = Encoding.UTF8.GetString(buffer, 0, received);
            Log.LogReceived(_logger, identifier, text);

            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(text, identifier)); // Handle user handlers error?

            token.ThrowIfCancellationRequested();
        }
    }

    private async Task HeartbeatLoop(string identifier, CancellationToken token)
    {
        TimeSpan interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);
        while (true)
        {
            OneBotStatus status = new(true, true);
            OneBotHeartBeat heartBeat = new(_context.BotUin, (int)_options.HeartBeatInterval, status);
            await SendJsonAsync(heartBeat, identifier, token);
            await Task.Delay(interval, token);

            token.ThrowIfCancellationRequested();
        }
    }

    public async ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken token = default)
    {
        IEnumerable<WebSocket> wss;

        if (identifier == null)
        {
            wss = _connections.Where(c =>
            {
                string localPath = c.Value.WsContext.RequestUri.LocalPath;
                return localPath != "/api" && localPath != "/api/";
            })
            .Select(c => c.Value.WsContext.WebSocket);
        }
        else wss = [_connections[identifier].WsContext.WebSocket];

        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(json);
        string jsonString = _logger.IsEnabled(LogLevel.Trace) ? Encoding.UTF8.GetString(jsonBytes) : string.Empty;
        foreach (WebSocket ws in wss)
        {
            Log.LogSend(_logger, identifier ?? string.Empty, jsonString);
            await ws.SendAsync(jsonBytes.AsMemory(), WebSocketMessageType.Text, true, token);
        }
    }

    private static bool IsPortInUse(uint port)
    {
        return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(endpoint => endpoint.Port == port);
    }
}

public partial class ForwardWSService
{
    private static partial class Log
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Connected(Conn: {identifier})")]
        public static partial void LogConnected(ILogger logger, string identifier);

        [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "Disconnected(Conn: {identifier})")]
        public static partial void LogErrorDisconnected(ILogger logger, string identifier, Exception e);


        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Disconnected(Conn: {identifier})")]
        public static partial void LogDisconnected(ILogger logger, string identifier);

        public static void LogReceived(ILogger logger, string identifier, string data)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                if (data.Length > 1024)
                {
                    data = string.Concat(data.AsSpan(0, 1024), "...", (data.Length - 1024).ToString(), "bytes");
                }
                InternalLogReceived(logger, identifier, data);
            }
        }

        [LoggerMessage(EventId = 3, Level = LogLevel.Trace, Message = "Receive(Conn: {identifier}): {s}")]
        private static partial void InternalLogReceived(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 4, Level = LogLevel.Trace, Message = "Send(Conn: {identifier}): {s}")]
        public static partial void LogSend(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 5, Level = LogLevel.Critical, Message = "The port {port} is in use, service failed to start")]
        public static partial void LogPortInUse(ILogger logger, uint port);

        [LoggerMessage(EventId = 6, Level = LogLevel.Critical, Message = "AuthFailed(Conn: {identifier})")]
        public static partial void LogAuthFailed(ILogger logger, string identifier);
    }
}
