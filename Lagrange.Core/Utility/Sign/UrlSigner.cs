using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Lagrange.Core.Utility.Sign;

internal class UrlSigner : SignProvider
{
    private readonly string? _signServer;

    private readonly HttpClient _client = new();

    public UrlSigner(string? url)
    {
        _signServer = url;
    }

    public override byte[]? Sign(string cmd, uint seq, byte[] body, out byte[]? e, out string? t)
    {

        e = null;
        t = null;

        if (!WhiteListCommand.Contains(cmd)) return null;
        if (_signServer == null) throw new Exception("Sign server is not configured");

        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_signServer),
            Content = JsonContent.Create(new JsonObject
            {
                { "cmd", cmd },
                { "seq", seq },
                { "src", Convert.ToHexString(body) }
            })
        };

        using var message = _client.Send(request);
        if (message.StatusCode != HttpStatusCode.OK) throw new Exception($"Signer server returned a {message.StatusCode}");
        var json = JsonDocument.Parse(message.Content.ReadAsStream()).RootElement;

        var valueJson = json.GetProperty("value");
        var extraJson = valueJson.GetProperty("extra");
        var tokenJson = valueJson.GetProperty("token");
        var signJson = valueJson.GetProperty("sign");

        string? token = tokenJson.GetString();
        string? extra = extraJson.GetString();
        e = extra != null ? Convert.FromHexString(extra) : Array.Empty<byte>();
        t = token != null ? Encoding.UTF8.GetString(Convert.FromHexString(token)) : "";
        string sign = signJson.GetString() ?? throw new Exception("Signer server returned an empty sign");
        return Convert.FromHexString(sign);
    }
}