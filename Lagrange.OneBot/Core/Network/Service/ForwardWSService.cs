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

    private readonly string _accessToken;

    public ForwardWSService(IConfiguration config, ILogger<LagrangeApp> logger, uint uin) : base(config, logger, uin)
    {
        string url = $"ws://{config["Host"]}:{config["Port"]}";

        _server = new WebSocketServer(url)
        {
            RestartAfterListenError = true
        };
        
        _timer = new Timer(OnHeartbeat, null, 1, config.GetValue<int>("HeartBeatInterval"));
        _accessToken = string.IsNullOrEmpty(config["AccessToken"]) ? "" : config["AccessToken"]!;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        uint port = Config.GetValue<uint?>("Port") ?? throw new Exception("Port is not defined");
        if (IsPortInUse(port))
        {
            Logger.LogCritical($"[{Tag}] The port {port} is in use, {Tag} failed to start");
            return Task.CompletedTask;
        }
        
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
                    if (!string.IsNullOrEmpty(_accessToken))
                    {
                        if (!conn.ConnectionInfo.Headers.ContainsKey("Authorization") || conn.ConnectionInfo.Headers["Authorization"] != $"Bearer {_accessToken}")
                        {
                            conn.Close(1002);
                            return;
                        }
                    }

                    Logger.LogInformation($"[{Tag}]: Connected");
                    
                    var lifecycle = new OneBotLifecycle(Uin, "connect");
                    SendJsonAsync(lifecycle, cancellationToken).GetAwaiter().GetResult();
                    
                    _timer.Change(0, Config.GetValue<int>("HeartBeatInterval"));
                };

                conn.OnClose = () =>
                {
                    Logger.LogWarning($"[{Tag}: Disconnected]");
                    _connection = null;
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