using System.Net.WebSockets;
using System.Text.Json;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Websocket.Client;
using Timer = System.Threading.Timer;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed class ReverseWSService : LagrangeWSService
{
    private const string Tag = nameof(ReverseWSService);
    public override event EventHandler<MsgRecvEventArgs>? OnMessageReceived = delegate { };
    
    private readonly WebsocketClient _socket;
    
    private readonly Timer _timer;
    
    public ReverseWSService(IConfiguration config, ILogger<LagrangeApp> logger) : base(config, logger)
    {
        var ws = Config.GetSection("Implementation").GetSection("ReverseWebSocket");
        string url = $"ws://{ws["Host"]}:{ws["Port"]}{ws["Suffix"]}";

        _socket = new WebsocketClient(new Uri(url), () =>
        {
            var socket = new ClientWebSocket();
            
            SetRequestHeader(socket, new Dictionary<string, string>
            {
                { "X-Client-Role", "Universal" },
                { "X-Self-ID", Config.GetValue<uint>("Account:Uin").ToString() },
                { "User-Agent", Constant.OneBotImpl }
            });
            socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            if (Config["AccessToken"] != null) socket.Options.SetRequestHeader("Authorization", $"Bearer {Config["AccessToken"]}");
            
            return socket;
        });
        
        _timer = new Timer(OnHeartbeat, null, -1, ws.GetValue<int>("HeartBeatInterval"));
        _socket.MessageReceived.Subscribe(resp =>
        {
            Logger.LogTrace($"[{Tag}] Receive: {resp.Text}");
            OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(resp.Text ?? ""));
        });
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _socket.Start();
        
        var lifecycle = new OneBotLifecycle(Config.GetValue<uint>("Account:Uin"), "connect");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _socket.Dispose();
        
        return Task.CompletedTask;
    }
    
    public override Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        string payload = JsonSerializer.Serialize(json);
        
        Logger.LogTrace($"[{Tag}] Send: {payload}");
        return _socket.SendInstant(payload);
    }

    private static void SetRequestHeader(ClientWebSocket webSocket, Dictionary<string, string> headers)
    {
        foreach (var (key, value) in headers) webSocket.Options.SetRequestHeader(key, value);
    }
}