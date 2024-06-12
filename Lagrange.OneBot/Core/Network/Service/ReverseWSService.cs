using System.Diagnostics;
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

    private string _urlStr = string.Empty;
    private readonly string _identifier = Guid.NewGuid().ToString();

    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly ReverseWSServiceOptions _options = options.Value;

    private readonly ILogger _logger = logger;

    protected ConnectionContext? ConnCtx;

    private readonly SemaphoreSlim semaphore = new(1, 1);

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

    protected async ValueTask SendJsonAsync<T>(ClientWebSocket ws, T payload, CancellationToken token)
    {
        var json = JsonSerializer.Serialize(payload);
        var buffer = Encoding.UTF8.GetBytes(json);
        Log.LogSendingData(_logger, Tag, _identifier, json);
        await semaphore.WaitAsync(token);
        try
        {
            await ws.SendAsync(buffer.AsMemory(), WebSocketMessageType.Text, true, token);
        }
        finally
        {
            semaphore.Release();
        }
    }

    protected ClientWebSocket CreateDefaultWebSocket(string role)
    {
        var ws = new ClientWebSocket();
        ws.Options.SetRequestHeader("X-Client-Role", role);
        ws.Options.SetRequestHeader("X-Self-ID", context.BotUin.ToString());
        ws.Options.SetRequestHeader("User-Agent", Constant.OneBotImpl);
        if (_options.AccessToken != null) ws.Options.SetRequestHeader("Authorization", $"Bearer {_options.AccessToken}");
        ws.Options.KeepAliveInterval = Timeout.InfiniteTimeSpan;
        return ws;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _urlStr = $"{_options.Host}:{_options.Port}{_options.Suffix}";
        if (!_options.Host.StartsWith("ws://") && !_options.Host.StartsWith("wss://"))
        {
            _urlStr = "ws://" + _urlStr;
        }
        string apiurlstr = $"{_urlStr}{_options.ApiSuffix}";
        string eventurlstr = $"{_urlStr}{_options.EventSuffix}";

        if (!Uri.TryCreate(_urlStr, UriKind.Absolute, out var url))
        {
            Log.LogInvalidUrl(_logger, Tag, _urlStr);
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
                    using var ws = CreateDefaultWebSocket("Universal");
                    var connTask = ws.ConnectAsync(url, stoppingToken);
                    using var connCtx = new UniversalConnectionContext(ws, connTask);
                    ConnCtx = connCtx;
                    await connTask;

                    Log.LogConnected(_logger, Tag, _identifier, _urlStr);
                    var lifecycle = new OneBotLifecycle(context.BotUin, "connect");
                    await SendJsonAsync(ws, lifecycle, stoppingToken);

                    var recvTask = ReceiveLoop(ws, stoppingToken);
                    if (_options.HeartBeatEnable && _options.HeartBeatInterval > 0)
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
                    using var wsApi = CreateDefaultWebSocket("Api");
                    var apiConnTask = wsApi.ConnectAsync(apiUrl, stoppingToken);

                    using var wsEvent = CreateDefaultWebSocket("Event");
                    var eventConnTask = wsEvent.ConnectAsync(eventUrl, stoppingToken);

                    var connTask = Task.WhenAll(apiConnTask, eventConnTask);
                    ConnCtx = new GeneralConnectionContext(wsApi, wsEvent, connTask);

                    await connTask;

                    var lifecycle = new OneBotLifecycle(context.BotUin, "connect");
                    await SendJsonAsync(wsEvent, lifecycle, stoppingToken);

                    var recvTask = ReceiveLoop(wsApi, stoppingToken);
                    if (_options.HeartBeatEnable && _options.HeartBeatInterval > 0)
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
                Log.LogClientReconnect(_logger, Tag, _identifier, _options.ReconnectInterval);
                var interval = TimeSpan.FromMilliseconds(_options.ReconnectInterval);
                await Task.Delay(interval, stoppingToken);
            }
            catch (Exception e)
            {
                Log.LogClientDisconnected(_logger, e, Tag, _identifier);
                var interval = TimeSpan.FromMilliseconds(_options.ReconnectInterval);
                await Task.Delay(interval, stoppingToken);
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
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", token);
                    break;
                }

                received += result.Count;
                if (result.EndOfMessage) break;

                if (received == buffer.Length) Array.Resize(ref buffer, received << 1);
            }
            string text = Encoding.UTF8.GetString(buffer, 0, received);
            Log.LogDataReceived(_logger, Tag, _identifier, text);
            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(text)); // Handle user handlers error?
        }
    }

    private async Task HeartbeatLoop(ClientWebSocket ws, CancellationToken token)
    {
        var interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);
        Stopwatch sw = new();

        while (true)
        {
            var status = new OneBotStatus(true, true);
            var heartBeat = new OneBotHeartBeat(context.BotUin, (int)_options.HeartBeatInterval, status);

            sw.Start();
            await SendJsonAsync(ws, heartBeat, token);
            sw.Stop();

            // Implementing precise intervals by subtracting Stopwatch's timing from configured intervals
            var waitingTime = interval - sw.Elapsed;
            if (waitingTime >= TimeSpan.Zero)
            {
                await Task.Delay(waitingTime, token);
            }
            sw.Reset();
        }
    }

    private static partial class Log
    {
        private enum EventIds
        {
            Connected = 1,
            SendingData,
            DataReceived,

            ClientDisconnected = 1001,
            ClientReconnect,
            InvalidUrl
        }

        [LoggerMessage(EventId = (int)EventIds.Connected, Level = LogLevel.Trace, Message = "[{tag}] Connect({identifier}): {url}")]
        public static partial void LogConnected(ILogger logger, string tag, string identifier, string url);

        [LoggerMessage(EventId = (int)EventIds.SendingData, Level = LogLevel.Trace, Message = "[{tag}] Send({identifier}): {data}")]
        public static partial void LogSendingData(ILogger logger, string tag, string identifier, string data);

        public static void LogDataReceived(ILogger logger, string tag, string identifier, string data)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                if (data.Length > 1024)
                {
                    data = string.Concat(data.AsSpan(0, 1024), "...", (data.Length - 1024).ToString(), "bytes");
                }
                InternalLogDataReceived(logger, tag, identifier, data);
            }
        }

        [LoggerMessage(EventId = (int)EventIds.DataReceived, Level = LogLevel.Trace, Message = "[{tag}] Receive({identifier}): {data}", SkipEnabledCheck = true)]
        private static partial void InternalLogDataReceived(ILogger logger, string tag, string identifier, string data);

        [LoggerMessage(EventId = (int)EventIds.ClientDisconnected, Level = LogLevel.Warning, Message = "[{tag}] Disconnect({identifier})")]
        public static partial void LogClientDisconnected(ILogger logger, Exception e, string tag, string identifier);

        [LoggerMessage(EventId = (int)EventIds.ClientReconnect, Level = LogLevel.Information, Message = "[{tag}] Reconnecting {identifier} at interval of {interval}")]
        public static partial void LogClientReconnect(ILogger logger, string tag, string identifier, uint interval);

        [LoggerMessage(EventId = (int)EventIds.InvalidUrl, Level = LogLevel.Error, Message = "[{tag}] Invalid configuration was detected, url: {url}")]
        public static partial void LogInvalidUrl(ILogger logger, string tag, string url);
    }
}
