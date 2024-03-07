using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Text.Json;
using Fleck;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Meta;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lagrange.OneBot.Core.Network.Service;

public sealed partial class ForwardWSService : ILagrangeWebService
{
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly ForwardWSServiceOptions _options;

    private readonly ILogger _logger;

    private readonly BotContext _context;

    private readonly WebSocketServer _server;

    private readonly ConcurrentDictionary<string, IWebSocketConnection> _connections;

    private readonly ConcurrentDictionary<string, Timer> _heartbeats;

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
        _connections = new ConcurrentDictionary<string, IWebSocketConnection>();

        _heartbeats = new ConcurrentDictionary<string, Timer>();
        _accessToken = _options.AccessToken ?? "";
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        uint port = _options.Port;
        if (IsPortInUse(port))
        {
            Log.LogPortInUse(_logger, port);
            return Task.CompletedTask;
        }

        return Task.Run(() =>
        {
            _server.Start(conn =>
            {
                string identifier = conn.ConnectionInfo.Id.ToString();

                conn.OnMessage = s =>
                {
                    Log.LogReceived(_logger, identifier, s);
                    if (conn.ConnectionInfo.Path == "/event") return;
                    OnMessageReceived?.Invoke(this, new MsgRecvEventArgs(s, identifier));
                };

                conn.OnOpen = () =>
                {
                    if (!string.IsNullOrEmpty(_accessToken))
                    {
                        if (!conn.ConnectionInfo.Headers.TryGetValue("Authorization", out string? value) || value != $"Bearer {_accessToken}")
                        {
                            Log.LogAuthFailed(_logger, identifier);
                            conn.Close(1002);
                            return;
                        }
                    }

                    _connections.TryAdd(identifier, conn);
                    Log.LogConnected(_logger, identifier);

                    if (conn.ConnectionInfo.Path == "/api") return;

                    var lifecycle = new OneBotLifecycle(_context.BotUin, "connect");
                    SendJsonAsync(lifecycle, identifier, cancellationToken).GetAwaiter().GetResult();

                    var timer = new Timer(x => OnHeartbeat(x, identifier), null, 1, _options.HeartBeatInterval);
                    timer.Change(0, _options.HeartBeatInterval);
                    _heartbeats.TryAdd(identifier, timer);
                };

                conn.OnClose = () =>
                {
                    Log.LogDisconnected(_logger, identifier);
                    _connections.TryRemove(identifier, out _);
                    if (_heartbeats.TryRemove(identifier, out var timer)) timer.Dispose();
                };
            });
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _server.ListenerSocket.Close();
        _server.Dispose();
        foreach (var (_, value) in _heartbeats) value.Dispose();

        return Task.CompletedTask;
    }

    public async ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken cancellationToken = default)
    {
        string payload = JsonSerializer.Serialize(json);
        Log.LogSend(_logger, identifier ?? string.Empty, payload);

        if (identifier == null)
        {
            foreach (var (_, connection) in _connections.Where(conn => conn.Value.ConnectionInfo.Path != "/api")) await connection.Send(payload);
        }
        else
        {
            await (_connections.TryGetValue(identifier, out var connection)
                ? new ValueTask(connection.Send(payload))
                : ValueTask.CompletedTask);
        }
    }

    private void OnHeartbeat(object? _, string identifier)
    {
        try
        {
            var status = new OneBotStatus(true, true);
            var heartBeat = new OneBotHeartBeat(_context.BotUin, (int)_options.HeartBeatInterval, status);

            SendJsonAsync(heartBeat, identifier).GetAwaiter().GetResult();
        }
        catch
        {
            // ignored
        }
    }

    private static bool IsPortInUse(uint port) =>
        IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(endpoint => endpoint.Port == port);

    private static partial class Log
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Connected(Conn: {identifier})")]
        public static partial void LogConnected(ILogger logger, string identifier);

        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Disconnected(Conn: {identifier})")]
        public static partial void LogDisconnected(ILogger logger, string identifier);

        [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = "Receive(Conn: {identifier}): {s}")]
        public static partial void LogReceived(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 3, Level = LogLevel.Trace, Message = "Send(Conn: {identifier}): {s}")]
        public static partial void LogSend(ILogger logger, string identifier, string s);

        [LoggerMessage(EventId = 4, Level = LogLevel.Critical, Message = "The port {port} is in use, service failed to start")]
        public static partial void LogPortInUse(ILogger logger, uint port);

        [LoggerMessage(EventId = 5, Level = LogLevel.Critical, Message = "Conn: {identifier} auth failed")]
        public static partial void LogAuthFailed(ILogger logger, string identifier);
    }
}