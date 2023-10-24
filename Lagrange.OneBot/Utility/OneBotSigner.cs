using System.Diagnostics.CodeAnalysis;
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
    private const string Tag = nameof(OneBotSigner);
    private readonly string _signServer;
    private readonly HttpClient _client;
    private readonly ILogger _logger;
    
    public OneBotSigner(IConfiguration config, ILogger<LagrangeApp> logger)
    {
        _signServer = config["SignServerUrl"] ?? "";
        _client = new HttpClient();
        _logger = logger;
        
        if (string.IsNullOrEmpty(_signServer))
        {
            Available = false;
            logger.LogWarning($"[{Tag}]: Signature Service is not available, login may be failed");
        }
        else
        {
            logger.LogInformation($"[{Tag}]: Signature Service is successfully established");
        }
    }

    public override byte[]? Sign(string cmd, uint seq, byte[] body, [UnscopedRef] out byte[]? ver, [UnscopedRef] out string? token)
    {
        ver = null;
        token = null;
        if (!WhiteListCommand.Contains(cmd)) return null;
        if (!Available || string.IsNullOrEmpty(_signServer)) return new byte[35]; // Dummy signature
        
        var payload = new Dictionary<string, string>
        {
            { "cmd", cmd },
            { "seq", seq.ToString() },
            { "src", body.Hex() },
        };

        try
        {
            string response = Http.GetAsync(_signServer, payload).GetAwaiter().GetResult();
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            ver = json?["value"]?["extra"]?.ToString().UnHex() ?? Array.Empty<byte>();
            token = Encoding.ASCII.GetString(json?["value"]?["token"]?.ToString().UnHex() ?? Array.Empty<byte>());
            return json?["value"]?["sign"]?.ToString().UnHex() ?? new byte[35];
        }
        catch
        {
            Available = false;
            _logger.LogWarning($"[{Tag}] Failed to get signature, using dummy signature");
            return new byte[35]; // Dummy signature
        }
    }
}