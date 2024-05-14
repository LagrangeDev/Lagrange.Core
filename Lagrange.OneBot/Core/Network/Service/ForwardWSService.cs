using System.Collections.Concurrent;
using System.Diagnostics;
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

public partial class ForwardWSService(ILogger<ForwardWSService> logger, IOptionsSnapshot<ForwardWSServiceOptions> options, BotContext context) : BackgroundService, ILagrangeWebService
{
    #region Initialization

    private readonly ForwardWSServiceOptions _options = options.Value;

    #endregion

    #region Lifecycle
    private readonly HttpListener _listener = new();

    public override Task StartAsync(CancellationToken token)
    {
        string host = _options.Host == "0.0.0.0" ? "*" : _options.Host;

        // First start the HttpListener
        _listener.Prefixes.Add($"http://{host}:{_options.Port}/");
        _listener.Start();

        foreach (string prefix in _listener.Prefixes) Log.LogServerStarted(logger, prefix);

        // then obtain the HttpListenerContext
        return base.StartAsync(token);
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            while (true) // Looping to Retrieve and Handle HttpListenerContext
            {
                _ = HandleHttpListenerContext(await _listener.GetContextAsync().WaitAsync(token), token);

                token.ThrowIfCancellationRequested();
            }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Log.LogWaitConnectException(logger, e);
        }
    }

    public override async Task StopAsync(CancellationToken token)
    {
        // Get the task of all currently connected tcs before stopping it
        var tasks = _connections.Values.Select(c => c.Tcs.Task).ToArray();

        // Stop obtaining the HttpListenerContext first
        await base.StopAsync(token);

        // Wait for the connection task to stop
        await Task.WhenAll(tasks).WaitAsync(token);

        // then stop the HttpListener
        _listener.Stop();
    }
    #endregion

    #region Connect
    private readonly ConcurrentDictionary<string, ConnectionContext> _connections = [];

    private async Task HandleHttpListenerContext(HttpListenerContext context1, CancellationToken token)
    {
        // Generating an identifier for this context
        string identifier = Guid.NewGuid().ToString();

        var response = context1.Response;

        try
        {
            Log.LogConnect(logger, identifier);

            // Validating AccessToken
            if (!ValidatingAccessToken(context1))
            {
                Log.LogValidatingAccessTokenFail(logger, identifier);

                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Close();

                return;
            }

            // Validating whether it is a WebSocket request
            if (!context1.Request.IsWebSocketRequest)
            {
                Log.LogNotWebSocketRequest(logger, identifier);

                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Close();

                return;
            }

            // Upgrade to WebSocket
            var wsContext = await context1.AcceptWebSocketAsync(null).WaitAsync(token);

            // Building and store ConnectionContext
            var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            _connections.TryAdd(identifier, new(wsContext, cts));

            string path = wsContext.RequestUri.LocalPath;
            bool isApi = path is "/api" or "/api/";
            bool isEvent = path is "/event" or "/event/";

            // Only API interfaces do not require sending heartbeats
            if (!isApi)
            {
                // Send ConnectLifecycleMetaEvent
                await SendJsonAsync(new OneBotLifecycle(context.BotUin, "connect"), identifier, token);

                if (_options is { HeartBeatEnable: true, HeartBeatInterval: > 0 })
                {
                    _ = HeartbeatAsyncLoop(identifier, cts.Token);
                }
            }

            // The Event interface does not need to receive messages
            // but still needs to receive Close messages to close the connection
            _ = isEvent 
                ? WaitCloseAsyncLoop(identifier, cts.Token)
                : ReceiveAsyncLoop(identifier, cts.Token);  // The Universal interface requires receiving messages
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Log.LogHandleHttpListenerContextException(logger, identifier, e);

            // Attempt to send a 500 response code
            try
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Close();
            }
            catch
            {
                // ignored
            }
        }
    }

    private bool ValidatingAccessToken(HttpListenerContext httpContext)
    {
        // If AccessToken is not configured
        // then allow access unconditionally
        if (string.IsNullOrEmpty(_options.AccessToken)) return true;

        string? token = null;

        // Retrieve the Authorization request header
        string? authorization = httpContext.Request.Headers["Authorization"];
        // If the Authorization request header is not present
        // retrieve the access_token from the QueryString
        if (authorization == null) token = httpContext.Request.QueryString["access_token"];
        // If the Authorization authentication method is Bearer
        // then retrieve the AccessToken
        else if (authorization.StartsWith("Bearer ")) token = authorization["Bearer ".Length..];

        return token == _options.AccessToken;
    }
    #endregion

    #region Receive
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    public async Task ReceiveAsyncLoop(string identifier, CancellationToken token)
    {
        if (!_connections.TryGetValue(identifier, out ConnectionContext? connection)) return;

        try
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int received = 0;
                while (true)
                {
                    var resultTask = connection.WsContext.WebSocket.ReceiveAsync(buffer.AsMemory(received), default);

                    var result = !resultTask.IsCompleted ?
                        await resultTask.AsTask().WaitAsync(token) :
                        resultTask.Result;

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectAsync(identifier, WebSocketCloseStatus.NormalClosure, token);
                        return;
                    }

                    received += result.Count;

                    if (result.EndOfMessage) break;

                    if (received == buffer.Length) Array.Resize(ref buffer, buffer.Length << 1);

                    token.ThrowIfCancellationRequested();
                }
                string message = Encoding.UTF8.GetString(buffer.AsSpan(0, received));

                Log.LogReceive(logger, identifier, message);

                OnMessageReceived?.Invoke(this, new(message, identifier));

                token.ThrowIfCancellationRequested();
            }
        }
        catch (Exception e)
        {
            bool isCanceled = e is OperationCanceledException;

            if (!isCanceled) Log.LogReceiveException(logger, identifier, e);

            var status = WebSocketCloseStatus.NormalClosure;
            var t = default(CancellationToken);
            if (!isCanceled)
            {
                status = WebSocketCloseStatus.InternalServerError;
                t = token;
            }

            await DisconnectAsync(identifier, status, t);

            if (token.IsCancellationRequested) throw;
        }
        finally
        {
            connection.Cts.Cancel();
        }
    }

    public async Task WaitCloseAsyncLoop(string identifier, CancellationToken token)
    {
        if (!_connections.TryGetValue(identifier, out ConnectionContext? connection)) return;

        try
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                ValueTask<ValueWebSocketReceiveResult> resultTask = connection.WsContext.WebSocket
                        .ReceiveAsync(buffer.AsMemory(), default);

                ValueWebSocketReceiveResult result = !resultTask.IsCompleted ?
                    await resultTask.AsTask().WaitAsync(token) :
                    resultTask.Result;

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await DisconnectAsync(identifier, WebSocketCloseStatus.NormalClosure, token);
                    return;
                }

                token.ThrowIfCancellationRequested();
            }
        }
        catch (Exception e)
        {
            bool isCanceled = e is OperationCanceledException;

            if (!isCanceled) Log.LogWaitCloseException(logger, identifier, e);

            var status = WebSocketCloseStatus.NormalClosure;
            var t = default(CancellationToken);
            if (!isCanceled)
            {
                status = WebSocketCloseStatus.InternalServerError;
                t = token;
            }

            await DisconnectAsync(identifier, status, t);

            if (token.IsCancellationRequested) throw;
        }
        finally
        {
            connection.Cts.Cancel();
        }
    }
    #endregion

    #region Heartbeat
    public async Task HeartbeatAsyncLoop(string identifier, CancellationToken token)
    {
        if (!_connections.TryGetValue(identifier, out ConnectionContext? connection)) return;

        Stopwatch sw = new();
        TimeSpan interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);

        try
        {
            while (true)
            {
                sw.Start();
                var heartbeat = new OneBotHeartBeat(context.BotUin, (int)_options.HeartBeatInterval, new OneBotStatus(true, true));
                await SendJsonAsync(heartbeat, identifier, token);
                sw.Stop();

                // Implementing precise intervals by subtracting Stopwatch's timing from configured intervals
                var waitingTime = interval - sw.Elapsed;
                if (waitingTime >= TimeSpan.Zero) await Task.Delay(waitingTime, token);

                sw.Reset();

                token.ThrowIfCancellationRequested();
            }
        }
        catch (Exception e)
        {
            bool isCanceled = e is OperationCanceledException;

            if (!isCanceled) Log.LogHeartbeatException(logger, identifier, e);

            var status = WebSocketCloseStatus.NormalClosure;
            var t = default(CancellationToken);
            if (!isCanceled)
            {
                status = WebSocketCloseStatus.InternalServerError;
                t = token;
            }

            await DisconnectAsync(identifier, status, t);

            if (token.IsCancellationRequested) throw;
        }
        finally
        {
            connection.Cts.Cancel();
        }
    }
    #endregion

    #region Send
    public async ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken token = default)
    {
        byte[] payload = JsonSerializer.SerializeToUtf8Bytes(json);
        if (identifier != null)
        {
            await SendBytesAsync(payload, identifier, token);
        }
        else
        {
            await Task.WhenAll(_connections
                .Where(c =>
                {
                    string path = c.Value.WsContext.RequestUri.LocalPath;
                    return path != "/api" && path != "/api/";
                })
                .Select(c => SendBytesAsync(payload, c.Key, token))
            );
        }
    }

    public async Task SendBytesAsync(byte[] payload, string identifier, CancellationToken token)
    {
        if (!_connections.TryGetValue(identifier, out ConnectionContext? connection)) return;

        await connection.SendSemaphoreSlim.WaitAsync(token);

        try
        {
            Log.LogSend(logger, identifier, payload);
            await connection.WsContext.WebSocket.SendAsync(payload.AsMemory(), WebSocketMessageType.Text, true, token);
        }
        finally
        {
            connection.SendSemaphoreSlim.Release();
        }
    }
    #endregion

    #region Disconnect
    private async Task DisconnectAsync(string identifier, WebSocketCloseStatus status, CancellationToken token)
    {
        if (!_connections.TryRemove(identifier, out ConnectionContext? connection)) return;

        try
        {
            await connection.WsContext.WebSocket
                .CloseAsync(status, null, token)
                .WaitAsync(TimeSpan.FromSeconds(5), token);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            Log.LogDisconnectException(logger, identifier, e);
        }
        finally
        {
            Log.LogDisconnect(logger, identifier);

            connection.Tcs.SetResult();
        }
    }
    #endregion

    #region ConnectionContext
    public class ConnectionContext(WebSocketContext context, CancellationTokenSource cts)
    {
        public WebSocketContext WsContext { get; } = context;
        public SemaphoreSlim SendSemaphoreSlim { get; } = new(1);
        public CancellationTokenSource Cts { get; } = cts;
        public TaskCompletionSource Tcs { get; } = new();
    }
    #endregion

    #region Log
    public static partial class Log
    {
        #region Normal
        [LoggerMessage(EventId = 10, Level = LogLevel.Information, Message = "The server is started at {prefix}")]
        public static partial void LogServerStarted(ILogger logger, string prefix);

        [LoggerMessage(EventId = 11, Level = LogLevel.Information, Message = "Connect({identifier})")]
        public static partial void LogConnect(ILogger logger, string identifier);

        public static void LogReceive(ILogger logger, string identifier, string payload)
        {
            if (!logger.IsEnabled(LogLevel.Trace)) return;

            if (payload.Length > 1024) payload = $"{payload.AsSpan(0, 1024)} ...{payload.Length - 1024} bytes";

            InnerLogReceive(logger, identifier, payload);
        }

        [LoggerMessage(EventId = 12, Level = LogLevel.Trace, Message = "Receive({identifier}) {payload}", SkipEnabledCheck = true)]
        private static partial void InnerLogReceive(ILogger logger, string identifier, string payload);

        public static void LogSend(ILogger logger, string identifier, byte[] payload)
        {
            if (!logger.IsEnabled(LogLevel.Trace)) return;

            string payloadString = Encoding.UTF8.GetString(payload);

            if (payload.Length > 1024) payloadString = $"{payloadString.AsSpan(0, 1024)} ...{payloadString.Length - 1024} bytes";

            InnerLogSend(logger, identifier, payloadString);
        }

        [LoggerMessage(EventId = 13, Level = LogLevel.Trace, Message = "Send({identifier}) {payload}", SkipEnabledCheck = true)]
        private static partial void InnerLogSend(ILogger logger, string identifier, string payload);

        [LoggerMessage(EventId = 14, Level = LogLevel.Information, Message = "Disconnect({identifier})")]
        public static partial void LogDisconnect(ILogger logger, string identifier);
        #endregion

        #region Exception
        [LoggerMessage(EventId = 992, Level = LogLevel.Error, Message = "LogDisconnectException({identifier})")]
        public static partial void LogDisconnectException(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 993, Level = LogLevel.Error, Message = "LogHeartbeatException({identifier})")]
        public static partial void LogHeartbeatException(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 994, Level = LogLevel.Error, Message = "WaitCloseException({identifier})")]
        public static partial void LogWaitCloseException(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 995, Level = LogLevel.Error, Message = "ReceiveException({identifier})")]
        public static partial void LogReceiveException(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 996, Level = LogLevel.Warning, Message = "NotWebSocketRequest({identifier})")]
        public static partial void LogNotWebSocketRequest(ILogger logger, string identifier);

        [LoggerMessage(EventId = 997, Level = LogLevel.Warning, Message = "ValidatingAccessTokenFail({identifier})")]
        public static partial void LogValidatingAccessTokenFail(ILogger logger, string identifier);

        [LoggerMessage(EventId = 998, Level = LogLevel.Critical, Message = "HandleHttpListenerContextException({identifier})")]
        public static partial void LogHandleHttpListenerContextException(ILogger logger, string identifier, Exception e);

        [LoggerMessage(EventId = 999, Level = LogLevel.Critical, Message = "WaitConnectException")]
        public static partial void LogWaitConnectException(ILogger logger, Exception e);
        #endregion
    }
    #endregion
}
