using System.Text;
using System.Text.Json;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Meta;
using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service;

public partial class HttpPostService(IOptionsSnapshot<HttpPostServiceOptions> options, ILogger<HttpPostService> logger, BotContext context)
    : BackgroundService, ILagrangeWebService
{
    private const string Tag = nameof(HttpPostService);

    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived { add { } remove { } }

    private readonly HttpPostServiceOptions _options = options.Value;

    private readonly ILogger _logger = logger;

    private Uri? _url;

    private static readonly HttpClient _client = new();

    public async ValueTask SendJsonAsync<T>(T payload, string? identifier, CancellationToken cancellationToken = default)
    {
        if (_url is null) throw new InvalidOperationException("Reverse HTTP service was not running");

        var json = JsonSerializer.Serialize(payload);
        Log.LogSendingData(_logger, Tag, json);
        using var request = new HttpRequestMessage(HttpMethod.Post, _url)
        {
            Headers = { { "X-Self-ID", context.BotUin.ToString() } },
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        try
        {
            await _client.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Log.LogPostFailed(_logger, ex, Tag);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string urlstr = $"{_options.Host}:{_options.Port}{_options.Suffix}";
        if (!_options.Host.StartsWith("http://") && !_options.Host.StartsWith("https://"))
        {
            urlstr = "http://" + urlstr;
        }

        if (!Uri.TryCreate(urlstr, UriKind.Absolute, out _url))
        {
            Log.LogInvalidUrl(_logger, Tag, urlstr);
            return;
        }

        try
        {
            var lifecycle = new OneBotLifecycle(context.BotUin, "connect");
            await SendJsonAsync(lifecycle, null, stoppingToken);

            if (_options.HeartBeatInterval > 0)
            {
                _ = HeartbeatLoop(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            return;
        }
    }

    private async Task HeartbeatLoop(CancellationToken token)
    {
        var interval = TimeSpan.FromMilliseconds(_options.HeartBeatInterval);
        while (true)
        {
            var status = new OneBotStatus(true, true);
            var heartBeat = new OneBotHeartBeat(context.BotUin, (int)_options.HeartBeatInterval, status);
            await SendJsonAsync(heartBeat, null, token);
            await Task.Delay(interval, token);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Trace, Message = "[{tag}] Send: {data}")]
        public static partial void LogSendingData(ILogger logger, string tag, string data);

        [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "[{tag}] Post failed")]
        public static partial void LogPostFailed(ILogger logger, Exception ex, string tag);

        [LoggerMessage(EventId = 10, Level = LogLevel.Error, Message = "[{tag}] Invalid configuration was detected, url: {url}")]
        public static partial void LogInvalidUrl(ILogger logger, string tag, string url);
    }
}
