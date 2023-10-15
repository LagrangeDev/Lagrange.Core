using System.Text.Json;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebsocketsSimple.Server;
using WebsocketsSimple.Server.Events.Args;
using WebsocketsSimple.Server.Models;
using Timer = System.Threading.Timer;

namespace Lagrange.OneBot.Core.Network;

public sealed class ForwardWebSocketService : ILagrangeWebService
{
    public event EventHandler<string> OnMessageReceived = delegate { };

    private readonly WebsocketServer _server;

    private readonly IConfiguration _config;

    private readonly ILogger _logger;

    private readonly Timer _timer;

    private readonly bool _shouldAuthenticate;

    public ForwardWebSocketService(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _config = config;
        _logger = logger;

        var ws = _config.GetSection("Implementation").GetSection("ForwardWebSocket");

        _timer = new Timer(OnHeartbeat, null, int.MaxValue, ws.GetValue<int>("HeartBeatInterval"));
        _shouldAuthenticate = string.IsNullOrEmpty(ws["Authorization"]);

        _server = new WebsocketServer(new ParamsWSServer(ws.GetValue<int>("Port")));
        _server.MessageEvent += (_, e) => OnMessageReceived.Invoke(this, e.Message ?? "");
        // todo: realize sending return packets

        if (_shouldAuthenticate)
            _server.ConnectionEvent += (_, e) => OnConnectionEvent(e);
    }

    private void OnConnectionEvent(WSConnectionServerEventArgs e)
    {
        if (
            _shouldAuthenticate
            && e.ConnectionEventType == PHS.Networking.Enums.ConnectionEventType.Connected
            && (
                e.RequestHeaders is null
                || !e.RequestHeaders.TryGetValue("Authorization", out string? authorization)
                || authorization != _config["Implementation:ForwardWebSocket:Authorization"]
            )
        )
        {
            e.Connection.Websocket.Abort();
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _server.StartAsync(cancellationToken);
        var lifecycle = new OneBotLifecycle(_config.GetValue<uint>("Account:Uin"), "connect");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        _server.Dispose();

        return Task.CompletedTask;
    }

    public async Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(json);
        await _server.BroadcastToAllConnectionsAsync(payload);
    }

    private void OnHeartbeat(object? sender)
    {
        var status = new OneBotStatus(true, true);
        var heartBeat = new OneBotHeartBeat(
            _config.GetValue<uint>("Account:Uin"),
            _config.GetValue<int>("Implementation:ForwardWebSocket:HeartBeatInterval"),
            status
        );

        SendJsonAsync(heartBeat).GetAwaiter().GetResult();
    }
}
