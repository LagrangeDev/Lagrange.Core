using System.Net.NetworkInformation;
using System.Text.Json;
using Fleck;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Meta;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed class ForwardWSService : ILagrangeWebService
{
    private const string Tag = nameof(ForwardWSService);

    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly ForwardWSServiceOptions _options;

    private readonly ILogger _logger;

    private readonly BotContext _context;

    private readonly WebSocketServer _server;

    private IWebSocketConnection? _connection;
    
    private readonly Timer _timer;

    private readonly string _accessToken;

    public ForwardWSService(IOptionsSnapshot<ForwardWSServiceOptions> options, ILogger<ForwardWSService> logger, BotContext context)
    {
        _options = options.Value;
        _logger = logger;
        _context = context;
        _server = new WebSocketServer($"ws://{_options.Host}:{_options.Port}")
        {
            RestartAfterListenError = true
        };
        
        _timer = new Timer(OnHeartbeat, null, 1, _options.HeartBeatInterval);
        _accessToken = _options.AccessToken ?? "";
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        uint port = _options.Port;
        if (IsPortInUse(port))
        {
            _logger.LogCritical($"[{Tag}] The port {port} is in use, {Tag} failed to start");
            return Task.CompletedTask;
        }
        
        return Task.Run(() =>
        {
            _server.Start(conn =>
            {
                _connection = conn;
                
                conn.OnMessage = s =>
                {
                    _logger.LogTrace($"[{Tag}] Receive: {s}");
                    OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(s));
                };

                conn.OnOpen = () =>
                {
                    if (!string.IsNullOrEmpty(_accessToken))
                    {
                        if (!conn.ConnectionInfo.Headers.TryGetValue("Authorization", out string? value) || value != $"Bearer {_accessToken}")
                        {
                            conn.Close(1002);
                            return;
                        }
                    }

                    _logger.LogInformation($"[{Tag}]: Connected");
                    
                    var lifecycle = new OneBotLifecycle(_context.BotUin, "connect");
                    SendJsonAsync(lifecycle, cancellationToken).GetAwaiter().GetResult();
                    
                    _timer.Change(0, _options.HeartBeatInterval);
                };

                conn.OnClose = () =>
                {
                    _logger.LogWarning($"[{Tag}: Disconnected]");
                    _connection = null;
                };
            });
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _server.ListenerSocket.Close();
        _server.Dispose();
        
        return Task.CompletedTask;
    }
    
    public ValueTask SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        string payload = JsonSerializer.Serialize(json);
        _logger.LogTrace($"[{Tag}] Send: {payload}");

        var connection = _connection;
        return connection != null ? new ValueTask(connection.Send(payload)) : default;
    }

    private void OnHeartbeat(object? sender)
    {
        var status = new OneBotStatus(true, true);
        var heartBeat = new OneBotHeartBeat(_context.BotUin, (int)_options.HeartBeatInterval, status);

        _ = SendJsonAsync(heartBeat);
    }

    private static bool IsPortInUse(uint port)
    {
        return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(endpoint => endpoint.Port == port);
    }

}