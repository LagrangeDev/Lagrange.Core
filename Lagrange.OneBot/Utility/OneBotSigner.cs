using System.Diagnostics.CodeAnalysis;
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
    private readonly string _signServer;
    private readonly ILogger _logger;
    private readonly Timer _timer;

    private readonly HttpClient _client;

    public OneBotSigner(IConfiguration config, ILogger<OneBotSigner> logger)
    {
        _signServer = config["SignServerUrl"] ?? "";
        _logger = logger;
        _client = new HttpClient();
        
        if (string.IsNullOrEmpty(_signServer))
        {
            Available = false;
            logger.LogWarning("Signature Service is not available, login may be failed");
        }
        else
        {
            logger.LogInformation("Signature Service is successfully established");
        }
        
        _timer = new Timer(_ =>
        {
            bool reconnect = Available = Test();
            if (reconnect) _timer?.Change(-1, 5000);
        });
    }

    public override byte[]? Sign(string cmd, uint seq, byte[] body, [UnscopedRef] out byte[]? ver, [UnscopedRef] out string? token)
    {
        ver = null;
        token = null;
        if (!WhiteListCommand.Contains(cmd)) return null;
        if (!Available || string.IsNullOrEmpty(_signServer)) return new byte[35]; // Dummy signature
        
        var payload = new JsonObject
        {
            { "cmd", cmd },
            { "seq", seq },
            { "src", body.Hex() },
        };

        try
        {
            var message = _client.PostAsJsonAsync(_signServer, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            ver = json?["value"]?["extra"]?.ToString().UnHex() ?? Array.Empty<byte>();
            token = Encoding.ASCII.GetString(json?["value"]?["token"]?.ToString().UnHex() ?? Array.Empty<byte>());
            return json?["value"]?["sign"]?.ToString().UnHex() ?? new byte[35];
        }
        catch
        {
            Available = false;
            _timer.Change(0, 5000);
            
            _logger.LogWarning("Failed to get signature, using dummy signature");
            return new byte[35]; // Dummy signature
        }
    }

    public override bool Test()
    {
        try
        {
            string response = Http.GetAsync($"{_signServer}/ping").GetAwaiter().GetResult();
            if (JsonSerializer.Deserialize<JsonObject>(response)?["code"]?.GetValue<int>() == 0)
            {
                _logger.LogInformation("Reconnected to Signature Service successfully");
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }
}