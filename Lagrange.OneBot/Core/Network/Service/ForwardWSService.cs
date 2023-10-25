using System.Net.NetworkInformation;
using System.Text.Json;
using Fleck;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed class ForwardWSService : LagrangeWSService
{
    private const string Tag = nameof(ForwardWSService);
    public override event EventHandler<MsgRecvEventArgs>? OnMessageReceived = delegate {  };
    
    private readonly WebSocketServer _server;

    private IWebSocketConnection? _connection;
    
    private readonly Timer _timer;

    public ForwardWSService(IConfiguration config, ILogger<LagrangeApp> logger) : base(config, logger)
    {
        var ws = config.GetSection("Implementation").GetSection("ForwardWebSocket");
        string url = $"ws://{ws["Host"]}:{ws["Port"]}";

        _server = new WebSocketServer(url)
        {
            RestartAfterListenError = true
        };
        
        _timer = new Timer(OnHeartbeat, null, int.MaxValue, ws.GetValue<int>("HeartBeatInterval"));
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            _server.Start(conn =>
            {
                _connection = conn;
                
                conn.OnMessage = s =>
                {
                    Logger.LogTrace($"[{Tag}] Receive: {s}");
                    OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(s));
                };

                conn.OnOpen = () =>
                {
                    Logger.LogInformation($"[{Tag}]: Connected");
                    
                    var lifecycle = new OneBotLifecycle(Config.GetValue<uint>("Account:Uin"), "connect");
                    SendJsonAsync(lifecycle, cancellationToken).GetAwaiter().GetResult();
                };
            });
        }, cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _server.ListenerSocket.Close();
        _server.Dispose();
        
        return Task.CompletedTask;
    }
    
    public override Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        string payload = JsonSerializer.Serialize(json);
        
        Logger.LogTrace($"[{Tag}] Send: {payload}");
        return _connection?.Send(payload) ?? Task.CompletedTask;
    }
    
    private static bool IsPortInUse(uint port)
    {
        return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(endpoint => endpoint.Port == port);
    }

}