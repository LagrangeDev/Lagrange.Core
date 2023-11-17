using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Network.Service;

public class ReverseWSService : LagrangeWSService
{
    private const string Tag = nameof(ReverseWSService);
    
    public override event EventHandler<MsgRecvEventArgs>? OnMessageReceived = delegate { };

    private readonly ClientWebSocket _socket;
    
    private readonly Timer _timer;

    public ReverseWSService(IConfiguration config, ILogger<LagrangeApp> logger, uint uin) : base(config, logger, uin)
    {
        var socket = new ClientWebSocket();
        
        SetRequestHeader(socket, new Dictionary<string, string>
        {
            { "X-Client-Role", "Universal" },
            { "X-Self-ID", uin.ToString() },
            { "User-Agent", Constant.OneBotImpl }
        });
        if (string.IsNullOrEmpty(config["AccessToken"])) socket.Options.SetRequestHeader("Authorization", $"Bearer {config["AccessToken"]}");
        socket.Options.KeepAliveInterval = Timeout.InfiniteTimeSpan;
        _socket = socket;
        
        _timer = new Timer(OnHeartbeat, null, -1, config.GetValue<int>("HeartBeatInterval"));
    }


    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        string url = $"ws://{Config["Host"]}:{Config["Port"]}{Config["Suffix"]}";
        
        await _socket.ConnectAsync(new Uri(url), cancellationToken);
        _ = ReceiveLoop(cancellationToken);
        
        var lifecycle = new OneBotLifecycle(Uin, "connect");
        await SendJsonAsync(lifecycle, cancellationToken);

        _timer.Change(0, Config.GetValue<int>("HeartBeatInterval"));
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
        return _socket.SendAsync(Encoding.UTF8.GetBytes(payload), WebSocketMessageType.Text, true, cancellationToken);
    }

    private async Task ReceiveLoop(CancellationToken cancellationToken)
    {
        try
        {
            await Task.CompletedTask.ForceAsync();
            var buffer = new byte[64 * 1024 * 1024];
            while (true)
            {
                var result = await _socket.ReceiveAsync(buffer.AsMemory(), cancellationToken);
                byte[] newBuffer = new byte[result.Count];
                Unsafe.CopyBlock(ref newBuffer[0], ref buffer[0], (uint)result.Count);

                string text = Encoding.UTF8.GetString(newBuffer);
                Logger.LogTrace($"[{Tag}] Receive: {text}");
                OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(text));
            }
        }
        catch
        {
            // 
        }
    }
    
    private static void SetRequestHeader(ClientWebSocket webSocket, Dictionary<string, string> headers)
    {
        foreach (var (key, value) in headers) webSocket.Options.SetRequestHeader(key, value);
    }
}