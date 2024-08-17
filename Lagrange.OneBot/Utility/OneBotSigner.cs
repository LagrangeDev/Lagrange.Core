using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;
using Lagrange.Core.Utility.Sign;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Utility;

public class OneBotSigner : SignProvider
{
    private readonly string? _signServer;
    private readonly ILogger _logger;

    private readonly HttpClient _client;

    public OneBotSigner(IConfiguration config, ILogger<OneBotSigner> logger)
    {
        _signServer = config["SignServerUrl"] ?? "";
        _logger = logger;
        var signProxyUrl = config["SignProxyUrl"];
        //Only support HTTP proxy

        _client = new HttpClient(handler: new HttpClientHandler
        {
            Proxy = string.IsNullOrEmpty(signProxyUrl) ? null : new WebProxy()
            {
                Address = new Uri(signProxyUrl),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
            },
            UseProxy = !string.IsNullOrEmpty(signProxyUrl),
        },
        disposeHandler: true);

        if (string.IsNullOrEmpty(_signServer)) logger.LogWarning("Signature Service is not available, login may be failed");
    }

    public override byte[]? Sign(string cmd, uint seq, byte[] body, [UnscopedRef] out byte[]? e, [UnscopedRef] out string? t)
    {
        // result
        e = null;
        t = null;

        if (!WhiteListCommand.Contains(cmd)) return null;

        if (Random.Shared.Next() % 2 == 1) throw new Exception("Test Exception");

        if (_signServer == null) throw new Exception("Sign server is not configured");

        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post,
            RequestUri = new(_signServer),
            Content = new StringContent(
                $"{{\"cmd\":\"{cmd}\",\"seq\":{seq},\"src\":\"{Convert.ToHexString(body)}\"}}",
                new MediaTypeHeaderValue("application/json")
            )
        };

        HttpResponseMessage message = _client.Send(request);

        if (message.StatusCode != HttpStatusCode.OK) throw new Exception($"Signer server returned a {message.StatusCode}");

        JsonElement json = JsonDocument.Parse(message.Content.ReadAsStream()).RootElement;

        JsonElement valueJson = json.GetProperty("value");

        JsonElement extraJson = valueJson.GetProperty("extra");
        JsonElement tokenJson = valueJson.GetProperty("token");
        JsonElement signJson = valueJson.GetProperty("sign");

        string? extra = extraJson.GetString();
        e = extra != null ? Convert.FromHexString(extra) : [];

        string? token = tokenJson.GetString();
        t = token != null ? Encoding.UTF8.GetString(Convert.FromHexString(token)) : "";

        string sign = signJson.GetString() ?? throw new Exception("Signer server returned an empty sign");
        return Convert.FromHexString(sign);
    }
}