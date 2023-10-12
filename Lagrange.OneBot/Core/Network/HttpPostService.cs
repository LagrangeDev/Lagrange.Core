using System.Text;
using System.Text.Json;
using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Network;

public sealed class HttpPostService : ILagrangeWebService
{
    public event EventHandler<string>? OnMessageReceived = delegate { };
    
    private readonly HttpClient _client;
    
    private readonly IConfiguration _config;
    
    private readonly ILogger _logger;

    private readonly string _url;
    
    public HttpPostService(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _config = config;
        _logger = logger;
        _client = new HttpClient();
        
        _client.DefaultRequestHeaders.Add("User-Agent", "Lagrange");
        _client.DefaultRequestHeaders.Add("X-Self-ID", _config.GetValue<uint>("Account:Uin").ToString());
        
        var conf = _config.GetSection("Implementation").GetSection("HttpPost");
        _url = $"http://{conf["Host"]}:{conf["Port"]}{conf["Suffix"]}";
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var lifecycle = new OneBotLifecycle(_config.GetValue<uint>("Account:Uin"), "enable");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var lifecycle = new OneBotLifecycle(_config.GetValue<uint>("Account:Uin"), "disable");
        await SendJsonAsync(lifecycle, cancellationToken);
    }

    public async Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        string payload = JsonSerializer.Serialize(json);
        await _client.PostAsync(_url, new StringContent(payload, Encoding.UTF8), cancellationToken);
    }
}