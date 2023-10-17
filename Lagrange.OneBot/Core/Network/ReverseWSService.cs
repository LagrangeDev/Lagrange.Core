using System.Text;
using System.Text.Json;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PHS.Networking.Enums;
using WebsocketsSimple.Client;
using WebsocketsSimple.Client.Events.Args;
using WebsocketsSimple.Client.Models;
using Timer = System.Threading.Timer;

namespace Lagrange.OneBot.Core.Network;

public sealed class ReverseWSService : ILagrangeWebService
{
    public event EventHandler<MsgRecvEventArgs> OnMessageReceived = delegate { };

    private readonly WebsocketClient _websocketClient;

    private readonly IConfiguration _config;

    private readonly ILogger _logger;

    private readonly Timer _timer;

    private static readonly Encoding _utf8 = new UTF8Encoding(false);

    public ReverseWSService(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _config = config;
        _logger = logger;

        var ws = _config.GetSection("Implementation").GetSection("ReverseWebSocket");

        var headers = new Dictionary<string, string>()
        {
            { "X-Client-Role", "Universal" },
            { "X-Self-ID", _config.GetValue<uint>("Account:Uin").ToString() },
            { "User-Agent", Constant.OneBotImpl }
        };

        if (_config["AccessToken"] != null)
            headers.Add("Authorization", $"Bearer {_config["AccessToken"]}");

        _websocketClient = new WebsocketClient(
            new ParamsWSClient(
                host: ws["Host"],
                port: ws.GetValue<int>("Port"),
                path: ws["Suffix"],
                isWebsocketSecured: false,
                keepAliveIntervalSec: ws.GetValue<int>("ReconnectInterval") / 1000,
                requestHeaders: headers
            )
        );
        _websocketClient.MessageEvent += OnMessage;

        _timer = new Timer(OnHeartbeat, null, int.MaxValue, ws.GetValue<int>("HeartBeatInterval"));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _websocketClient.ConnectAsync();

        var lifecycle = new OneBotLifecycle(_config.GetValue<uint>("Account:Uin"), "connect");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        await _websocketClient.DisconnectAsync();
    }

    public Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(json);
        return _websocketClient.SendAsync(payload);
    }

    private void OnHeartbeat(object? sender)
    {
        var status = new OneBotStatus(true, true);
        var heartBeat = new OneBotHeartBeat(
            _config.GetValue<uint>("Account:Uin"),
            _config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"),
            status
        );

        SendJsonAsync(heartBeat);
    }

    private void OnMessage(object sender, WSMessageClientEventArgs e)
    {
        if (e.MessageEventType == MessageEventType.Receive)
        {
            string text = _utf8.GetString(e.Bytes);
            OnMessageReceived.Invoke(this, new(e.Message ?? ""));
        }
    }
}
