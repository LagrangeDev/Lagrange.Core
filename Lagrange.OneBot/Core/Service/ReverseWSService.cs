using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Timer = System.Threading.Timer;

namespace Lagrange.OneBot.Core.Service;

public sealed class ReverseWSService : ILagrangeWebService
{
    private readonly ClientWebSocket _socket;

    private readonly IConfiguration _config;
    
    private readonly ILogger _logger;
    
    private readonly Timer _timer;
    
    public ReverseWSService(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _config = config;
        _logger = logger;
        _socket = new ClientWebSocket();
        _timer = new Timer(OnHeartbeat, null, int.MaxValue, config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"));

        _ = ReceiveLoop();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ConnectAsync(_config.GetValue<uint>("Account:Uin"), cancellationToken);
        
        var lifecycle = new OneBotLifecycle(_config.GetValue<uint>("Account:Uin"), "connect");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _socket.Dispose();
        
        return Task.CompletedTask;
    }

    public ValueTask SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(json);
        return _socket.SendAsync(payload.AsMemory(), WebSocketMessageType.Text, true, cancellationToken);
    }

    private async Task<bool> ConnectAsync(uint botUin, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Connecting to reverse websocket...");
        
        var config = _config.GetSection("Implementation").GetSection("ReverseWebSocket");
        string url = $"ws://{config["Host"]}:{config["Port"]}{config["Suffix"]}";
        
        _socket.Options.SetRequestHeader("X-Client-Role", "Universal");
        _socket.Options.SetRequestHeader("X-Self-ID", botUin.ToString());
        _socket.Options.SetRequestHeader("User-Agent", Constant.OneBotImpl);
        if (_config["AccessToken"] != null) _socket.Options.SetRequestHeader("Authorization", $"Bearer {_config["AccessToken"]}");
        
        await _socket.ConnectAsync(new Uri(url), cancellationToken);
        _timer.Change(0, _config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"));
        
        return true;
    }

    private void OnHeartbeat(object? sender)
    {
        var status = new OneBotStatus(true, true);
        var heartBeat = new OneBotHeartBeat(_config.GetValue<uint>("Account:Uin"), _config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"), status);
        
        SendJsonAsync(heartBeat);
    }
    
    private async Task ReceiveLoop()
    {
        while (true)
        {
            await Task.CompletedTask.ForceAsync();

            if (_socket.State == WebSocketState.Open)
            {
                var buffer = new byte[64 * 1024 * 1024]; // 64MB
                var result = await _socket.ReceiveAsync(buffer.AsMemory(), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    break;
                }

                string payload = Encoding.UTF8.GetString(buffer[..result.Count]);
                _logger.LogInformation(payload);
            }
        }
    }
}