using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Meta;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public partial class ReverseWSService(IOptionsSnapshot<ReverseWSServiceOptions> options, ILogger<ReverseWSService> logger, BotContext context)
    : BackgroundService, ILagrangeWebService
{
    private const string Tag = nameof(ReverseWSService);

    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly ReverseWSServiceOptions _options = options.Value;

    private readonly ILogger _logger = logger;

    protected ConnectionContext? ConnCtx;

    protected abstract class ConnectionContext(Task connectTask) : IDisposable
    {
        public readonly Task ConnectTask = connectTask;

        private readonly CancellationTokenSource _cts = new();

        public CancellationToken Token => _cts.Token;

        public void Dispose() => _cts.Cancel();
    }

    protected sealed class GeneralConnectionContext(ClientWebSocket apiWebSocket, ClientWebSocket eventWebSocket, Task connectTask) : ConnectionContext(connectTask)
    {
        public readonly ClientWebSocket ApiWebSocket = apiWebSocket;
        public readonly ClientWebSocket EventWebSocket = eventWebSocket;
    }

    protected sealed class UniversalConnectionContext(ClientWebSocket webSocket, Task connectTask) : ConnectionContext(connectTask)
    {
        public readonly ClientWebSocket WebSocket = webSocket;
    }

    public ValueTask SendJsonAsync<T>(T payload, string? identifier, CancellationToken cancellationToken = default)
    {
        var ctx = ConnCtx ?? throw new InvalidOperationException("Reverse webSocket service was not running");
        var ws = ctx switch
        {
            UniversalConnectionContext universalCtx => universalCtx.WebSocket,
            GeneralConnectionContext generalCtx => payload is OneBotResult ? generalCtx.ApiWebSocket : generalCtx.EventWebSocket,
            _ => throw new InvalidOperationException("The connection context is not supported")
        };
        var connTask = ctx.ConnectTask;

        return !connTask.IsCompletedSuccessfully
            ? SendJsonAsync(ws, connTask, payload, ctx.Token)
            : SendJsonAsync(ws, payload, ctx.Token);
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
        ws.Options.SetRequestHeader("X-Self-ID", context.BotUin.ToString());
        ws.Options.SetRequestHeader("User-Agent", Constant.OneBotImpl);
        if (_options.AccessToken != null) ws.Options.SetRequestHeader("Authorization", $"Bearer {_options.AccessToken}");
        ws.Options.KeepAliveInterval = Timeout.InfiniteTimeSpan;
        return ws;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string urlstr = $"{_options.Host}:{_options.Port}{_options.Suffix}";
        if (!_options.Host.StartsWith("ws://") && !_options.Host.StartsWith("wss://"))
        {
            urlstr = "ws://" + urlstr;
        }
        string apiurlstr = $"{urlstr}{_options.ApiSuffix}";
        string eventurlstr = $"{urlstr}{_options.EventSuffix}";

        if (!Uri.TryCreate(urlstr, UriKind.Absolute, out var url))
        {
            Log.LogInvalidUrl(_logger, Tag, urlstr);
            return;
        }
        if (!Uri.TryCreate(apiurlstr, UriKind.Absolute, out var apiUrl))
        {
            Log.LogInvalidUrl(_logger, Tag, apiurlstr);
            return;
        }
        if (!Uri.TryCreate(eventurlstr, UriKind.Absolute, out var eventUrl))
        {
            Log.LogInvalidUrl(_logger, Tag, eventurlstr);
            return;
        }

        while (true)
        {
            try
            {
                if (_options.UseUniversalClient)
                {
                    using var ws = CreateDefaultWebSocket();
                    var connTask = ws.ConnectAsync(url, stoppingToken);
                    using var connCtx = new UniversalConnectionContext(ws, connTask);
                    ConnCtx = connCtx;
                    await connTask;

                    var lifecycle = new OneBotLifecycle(context.BotUin, "connect");
                    await SendJsonAsync(ws, lifecycle, stoppingToken);

                    var recvTask = ReceiveLoop(ws, stoppingToken);
                    if (_options.HeartBeatInterval > 0)
                    {
                        var heartbeatTask = HeartbeatLoop(ws, stoppingToken);
                        await Task.WhenAll(recvTask, heartbeatTask);
                    }
                    else
                    {
                        await recvTask;
                    }
                }
                else
                {
                    using var wsApi = CreateDefaultWebSocket();
                    var apiConnTask = wsApi.ConnectAsync(apiUrl, stoppingToken);

                    using var wsEvent = CreateDefaultWebSocket();
                    var eventConnTask = wsEvent.ConnectAsync(eventUrl, stoppingToken);

                    var connTask = Task.WhenAll(apiConnTask, eventConnTask);
                    ConnCtx = new GeneralConnectionContext(wsApi, wsEvent, connTask);

                    await connTask;

                    var lifecycle = new OneBotLifecycle(context.BotUin, "connect");
                    await SendJsonAsync(wsEvent, lifecycle, stoppingToken);

                    var recvTask = ReceiveLoop(wsApi, stoppingToken);
                    if (_options.HeartBeatInterval > 0)
                    {
                        var heartbeatTask = HeartbeatLoop(wsEvent, stoppingToken);
                        await Task.WhenAll(recvTask, heartbeatTask);
                    }
                    else
                    {
                        await recvTask;
                    }
                }

            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                ConnCtx = null;
                break;
            }
            catch (WebSocketException e) when (e.InnerException is HttpRequestException)
            {
                Log.LogClientReconnect(_logger, Tag, _options.ReconnectInterval);
                var interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);
                await Task.Delay(interval, stoppingToken);
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
            int received = 0;
            while (true)
            {
                var result = await ws.ReceiveAsync(buffer.AsMemory(received), token);
                received += result.Count;
                if (result.EndOfMessage) break;

                if (received == buffer.Length) Array.Resize(ref buffer, received + 1024);
            }
            string text = Encoding.UTF8.GetString(buffer, 0, received);
            Log.LogDataReceived(_logger, Tag, text);
            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(text)); // Handle user handlers error?
        }
    }

    private async Task HeartbeatLoop(ClientWebSocket ws, CancellationToken token)
    {
        var interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);
        while (true)
        {
            var status = new OneBotStatus(true, true);
            var heartBeat = new OneBotHeartBeat(context.BotUin, (int)_options.HeartBeatInterval, status);
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

        [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "[{tag}] Client reconnecting at interval of {interval}")]
        public static partial void LogClientReconnect(ILogger logger, string tag, uint interval);

        [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "[{tag}] Client connect failed, reconnect after {interval} millisecond")]
        public static partial void LogConnectFailed(ILogger logger, string tag, uint interval);

        [LoggerMessage(EventId = 10, Level = LogLevel.Error, Message = "[{tag}] Invalid configuration was detected, url: {url}")]
        public static partial void LogInvalidUrl(ILogger logger, string tag, string url);
    }
}
