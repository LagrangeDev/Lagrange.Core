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
    private ConcurrentDictionary<string, ForwardWSServiceConnectionContext> _connections = [];

    public ForwardWSService(ILogger<ForwardWSService> logger, IOptionsSnapshot<ForwardWSServiceOptions> options, BotContext context)
    {
        _logger = logger;
        _options = options.Value;
        _context = context;

        _listener.Prefixes.Add($"http://{_options.Host}:{_options.Port}/");
    }

    private partial class ForwardWSServiceConnectionContext(WebSocket ws)
    {
        public WebSocket Ws { get; } = ws;
        public Task? ConnectionTask { get; set; }
    }

    private partial class ForwardWSServiceConnectionContext : IDisposable
    {
        public void Dispose()
        {
            Ws.Dispose();
            ConnectionTask?.Dispose();

            GC.SuppressFinalize(this);
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
        while (!token.IsCancellationRequested)
        {
            HttpListenerContext httpContext = await _listener.GetContextAsync().WaitAsync(token);
            string identifier = Guid.NewGuid().ToString();
            if (httpContext.Request.IsWebSocketRequest)
            {
                if (_options.AccessToken != "")
                {
                    string? accessToken = null;

                    string? authorization = httpContext.Request.Headers["Authorization"];
                    if (authorization == null)
                    {
                        accessToken = httpContext.Request.QueryString["access_token"];
                    }
                    else if (authorization.StartsWith("Bearer "))
                    {
                        accessToken = authorization[authorization.IndexOf("Bearer ")..];
                    }

                    if (accessToken == null)
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
                        return;
                    }
                }

                HttpListenerWebSocketContext wsContext = await httpContext.AcceptWebSocketAsync(null).WaitAsync(token);
                ForwardWSServiceConnectionContext connectionContext = new(wsContext.WebSocket);
                _connections[identifier] = connectionContext;
                connectionContext.ConnectionTask = ConnectionLoop(identifier, token);

                Log.LogConnected(_logger, identifier);
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                httpContext.Response.Close();
                return;
            }
        }
        token.ThrowIfCancellationRequested();
    }

    public override async Task StopAsync(CancellationToken token)
    {
        await base.StartAsync(token);
        _listener.Stop();
        return;
    }

    public async Task ConnectionLoop(string identifier, CancellationToken token)
    {
        try
        {
            await Task.WhenAll(ReceiveLoop(identifier, token), HeartbeatLoop(identifier, token));
            Log.LogDisconnected(_logger, identifier);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Log.LogErrorDisconnected(_logger, identifier, e);
        }
        finally
        {
            if (_connections.TryRemove(identifier, out ForwardWSServiceConnectionContext? connectionContext))
            {
                connectionContext?.Dispose();
            }
        }
    }

    private async Task ReceiveLoop(string identifier, CancellationToken token)
    {
        WebSocket ws = _connections[identifier].Ws;

        while (!token.IsCancellationRequested)
        {
            var buffer = new byte[1024];
            while (!token.IsCancellationRequested)
            {
                int received = 0;
                while (!token.IsCancellationRequested)
                {
                    var result = await ws.ReceiveAsync(buffer.AsMemory(received), token);
                    received += result.Count;
                    if (result.EndOfMessage) break;

                    if (received == buffer.Length) Array.Resize(ref buffer, received << 1);
                }
                token.ThrowIfCancellationRequested();
                string text = Encoding.UTF8.GetString(buffer, 0, received);
                Log.LogReceived(_logger, identifier, text);
                OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(text, identifier)); // Handle user handlers error?
            }
            token.ThrowIfCancellationRequested();
        }
        token.ThrowIfCancellationRequested();
    }

    private async Task HeartbeatLoop(string identifier, CancellationToken token)
    {
        var interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);
        while (true)
        {
            var status = new OneBotStatus(true, true);
            var heartBeat = new OneBotHeartBeat(_context.BotUin, (int)_options.HeartBeatInterval, status);
            await SendJsonAsync(heartBeat, identifier, token);
            await Task.Delay(interval, token);
        }
    }

    public async ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken token = default)
    {
        IEnumerable<WebSocket> wss;

        if (identifier == null) wss = _connections.Select(c => c.Value.Ws);
        else wss = [_connections[identifier].Ws];

        foreach (WebSocket ws in wss)
        {
            var jsonString = JsonSerializer.Serialize(json);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            Log.LogSend(_logger, identifier ?? string.Empty, jsonString);
            await ws.SendAsync(jsonBytes.AsMemory(), WebSocketMessageType.Text, true, token);
        }
    }

    private static bool IsPortInUse(uint port) =>
        IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(endpoint => endpoint.Port == port);
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

        [LoggerMessage(EventId = 6, Level = LogLevel.Critical, Message = "Conn: {identifier} auth failed")]
        public static partial void LogAuthFailed(ILogger logger, string identifier);
    }
}
