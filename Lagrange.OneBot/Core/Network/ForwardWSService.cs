using System.Text;
using System.Text.Json;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PHS.Networking.Enums;
using WebsocketsSimple.Server;
using WebsocketsSimple.Server.Events.Args;
using WebsocketsSimple.Server.Models;
using Timer = System.Threading.Timer;

namespace Lagrange.OneBot.Core.Network;

public sealed class ForwardWSService : ILagrangeWebService
{
    private const string Tag = nameof(ForwardWSService);

    public event EventHandler<MsgRecvEventArgs> OnMessageReceived = delegate { };

    private readonly WebsocketServer _server;

    private readonly IConfiguration _config;

    private readonly ILogger _logger;

    private readonly Timer _timer;

    private readonly bool _shouldAuthenticate;

    private static readonly Encoding _utf8 = new UTF8Encoding(false);

    public ForwardWSService(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _config = config;
        _logger = logger;

        var ws = _config.GetSection("Implementation").GetSection("ForwardWebSocket");

        _timer = new Timer(OnHeartbeat, null, int.MaxValue, ws.GetValue<int>("HeartBeatInterval"));
        _shouldAuthenticate = !string.IsNullOrEmpty(_config["AccessToken"]);

        _server = new WebsocketServer(new ParamsWSServer(ws.GetValue<int>("Port")));
        _server.MessageEvent += OnMessage;

        if (_shouldAuthenticate)
            _server.ConnectionEvent += OnConnection;
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
        var payload = JsonSerializer.Serialize(json);

        if (json is OneBotResult oneBotResult)
        {
            var connection = _server.Connections.FirstOrDefault(
                (c) => c.ConnectionId == oneBotResult.Identifier
            );

            if (connection is not null)
            {
                _logger.LogTrace($"[{Tag}] Send to {connection.ConnectionId}: {payload}");
                await _server.SendToConnectionAsync(payload, connection);
            }
            else
                _logger.LogWarning(
                    $"[{Tag}] Fail to send to {oneBotResult.Identifier} because the connection has been aborted."
                );
        }
        else
            await _server.BroadcastToAllConnectionsAsync(payload, cancellationToken);
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

    private async void OnConnection(object sender, WSConnectionServerEventArgs e)
    {
        if (
            _shouldAuthenticate
            && e.ConnectionEventType == ConnectionEventType.Connected
            && (
                e.RequestHeaders is null
                || !e.RequestHeaders.TryGetValue("Authorization", out string? authorization)
                || authorization != $"Bearer {_config["AccessToken"]}"
            )
        )
        {
            await _server.DisconnectConnectionAsync(e.Connection);
        }
    }

    private void OnMessage(object sender, WSMessageServerEventArgs e)
    {
        if (e.MessageEventType == MessageEventType.Receive)
        {
            string text = _utf8.GetString(e.Bytes);
            _logger.LogTrace($"[{Tag}] Receive from {e.Connection.ConnectionId}: {text}");
            OnMessageReceived.Invoke(this, new(e.Message ?? "", e.Connection.ConnectionId));
        }
    }
}
