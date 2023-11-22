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

public partial class ReverseWSService : BackgroundService, ILagrangeWebService
{
    protected const string Tag = nameof(ReverseWSService);
    
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    protected readonly ReverseWSServiceOptions _options;

    protected readonly ILogger _logger;

    protected readonly BotContext _botCtx;

    protected ConnectionContext? _connCtx;

    protected sealed class ConnectionContext : IDisposable
    {
        public readonly ClientWebSocket WebSocket;

        public readonly Task ConnectTask;

        private readonly CancellationTokenSource _cts;

        public CancellationToken Token => _cts.Token;

        public ConnectionContext(ClientWebSocket webSocket, Task connectTask)
        {
            WebSocket = webSocket;
            ConnectTask = connectTask;
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }

    public ReverseWSService(IOptionsSnapshot<ReverseWSServiceOptions> options, ILogger<LagrangeApp> logger, BotContext context)
    {
        _options = options.Value;
        _logger = logger;
        _botCtx = context;
    }

    public ValueTask SendJsonAsync<T>(T payload, CancellationToken cancellationToken = default)
    {
        var connCtx = _connCtx ?? throw new InvalidOperationException("Reverse webSocket service was not running");
        var connTask = connCtx.ConnectTask;
        if (!connTask.IsCompletedSuccessfully)
        {
            return SendJsonAsync(connCtx.WebSocket, connTask, payload, connCtx.Token);
        }
        return SendJsonAsync(connCtx.WebSocket, payload, connCtx.Token);
    }

    protected async ValueTask SendJsonAsync<T>(ClientWebSocket ws, Task connectTask, T payload, CancellationToken token)
    {
        await connectTask;
        await SendJsonAsync(ws, payload, token);
    }

    protected ValueTask SendJsonAsync<T>(ClientWebSocket ws, T payload, CancellationToken token)
    {
        var json = JsonSerializer.Serialize(payload);
        var buffer = Encoding.UTF8.GetBytes(json);
        Log.LogSendingData(_logger, Tag, json);
        return ws.SendAsync(buffer.AsMemory(), WebSocketMessageType.Text, true, token);
    }

    protected ClientWebSocket CreateDefaultWebSocket()
    {
        var ws = new ClientWebSocket();
        ws.Options.SetRequestHeader("X-Client-Role", "Universal");
        ws.Options.SetRequestHeader("X-Self-ID", _botCtx.BotUin.ToString());
        ws.Options.SetRequestHeader("User-Agent", Constant.OneBotImpl);
        if (_options.AccessToken != null)
        {
            ws.Options.SetRequestHeader("Authorization", $"Bearer {_options.AccessToken}");
        }
        ws.Options.KeepAliveInterval = Timeout.InfiniteTimeSpan;
        return ws;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string urlstr = $"ws://{_options.Host}:{_options.Port}{_options.Suffix}";
        if (!Uri.TryCreate(urlstr, UriKind.Absolute, out var url))
        {
            Log.LogInvalidUrl(_logger, Tag, urlstr);
            return;
        }
        while (true)
        {
            try
            {
                using var ws = CreateDefaultWebSocket();
                var connTask = ws.ConnectAsync(url, stoppingToken);
                using var connCtx = new ConnectionContext(ws, connTask);
                _connCtx = connCtx;
                await connTask;

                var lifecycle = new OneBotLifecycle(_botCtx.BotUin, "connect");
                await SendJsonAsync(ws, lifecycle, stoppingToken);

                var recvTask = ReceiveLoop(ws, stoppingToken);
                if (_options.HeartBeatInterval > 0)
                {
                    var heartbeatTask = HeartbeatLoop(ws, stoppingToken);
                    await Task.WhenAll([recvTask, heartbeatTask]);
                }
                else
                {
                    await recvTask;
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _connCtx = null;
                break;
            }
            catch (Exception e)
            {
                Log.LogClientDisconnected(_logger, e, Tag);
            }
        }
    }

    private async Task ReceiveLoop(ClientWebSocket ws, CancellationToken token)
    {
        var buffer = new byte[1024];
        while (true)
        {
            int rcvd = 0;
            while (true)
            {
                var result = await ws.ReceiveAsync(buffer.AsMemory(rcvd), token);
                if (result.EndOfMessage)
                {
                    break;
                }
                rcvd += result.Count;
                if (rcvd == buffer.Length)
                {
                    Array.Resize(ref buffer, rcvd + 1024);
                }
            }
            string text = Encoding.UTF8.GetString(buffer);
            Log.LogDataReceived(_logger, Tag, text);
            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(text)); // Handle user handlers error?
        }
    }

    private async Task HeartbeatLoop(ClientWebSocket ws, CancellationToken token)
    {
        var interval = TimeSpan.FromSeconds(_options.HeartBeatInterval);
        while (true)
        {
            var status = new OneBotStatus(true, true);
            var heartBeat = new OneBotHeartBeat(_botCtx.BotUin, (int)_options.HeartBeatInterval, status);
            await SendJsonAsync(ws, heartBeat, token);
            await Task.Delay(interval, token);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Trace, Message = "[{tag}] Send: {data}")]
        public static partial void LogSendingData(ILogger logger, string tag, string data);

        [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = "[{tag}] Receive: {data}")]
        public static partial void LogDataReceived(ILogger logger, string tag, string data);

        [LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "[{tag}] Client disconnected")]
        public static partial void LogClientDisconnected(ILogger logger, Exception e, string tag);

        [LoggerMessage(EventId = 10, Level = LogLevel.Error, Message = "[{tag}] Invalid configuration was detected, url: {url}")]
        public static partial void LogInvalidUrl(ILogger logger, string tag, string url);
    }
}