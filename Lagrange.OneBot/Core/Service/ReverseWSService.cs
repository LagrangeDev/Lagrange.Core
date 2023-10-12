using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Websocket.Client;
using Timer = System.Threading.Timer;

namespace Lagrange.OneBot.Core.Service;

public sealed class ReverseWSService : ILagrangeWebService
{
    private readonly WebsocketClient _socket;

    private readonly IConfiguration _config;
    
    private readonly ILogger _logger;
    
    private readonly Timer _timer;
    
    public ReverseWSService(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _config = config;
        _logger = logger;
        
        var ws = _config.GetSection("Implementation").GetSection("ReverseWebSocket");
        string url = $"ws://{ws["Host"]}:{ws["Port"]}{ws["Suffix"]}";

        _socket = new WebsocketClient(new Uri(url), () =>
        {
            var socket = new ClientWebSocket();
            
            SetRequestHeader(socket, new Dictionary<string, string>
            {
                { "X-Client-Role", "Universal" },
                { "X-Self-ID", _config.GetValue<uint>("Account:Uin").ToString() },
                { "User-Agent", Constant.OneBotImpl }
            });
            socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            if (_config["AccessToken"] != null) socket.Options.SetRequestHeader("Authorization", $"Bearer {_config["AccessToken"]}");
            
            return socket;
        });
        
        _timer = new Timer(OnHeartbeat, null, int.MaxValue, config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"));
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _socket.Start();
        
        var lifecycle = new OneBotLifecycle(_config.GetValue<uint>("Account:Uin"), "connect");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _socket.Dispose();
        
        return Task.CompletedTask;
    }

    public Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(json);
        Console.WriteLine(Encoding.UTF8.GetString(payload));
        return _socket.SendInstant(payload);
    }

    private void OnHeartbeat(object? sender)
    {
        var status = new OneBotStatus(true, true);
        var heartBeat = new OneBotHeartBeat(
            _config.GetValue<uint>("Account:Uin"), 
            _config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"), 
            status);
        
        SendJsonAsync(heartBeat);
    }

    private static void SetRequestHeader(ClientWebSocket webSocket, Dictionary<string, string> headers)
    {
        foreach (var (key, value) in headers) webSocket.Options.SetRequestHeader(key, value);
    }
}