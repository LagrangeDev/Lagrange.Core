using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;

namespace Lagrange.Core.Utility.Sign;

internal class LinuxSigner : SignProvider
{
    private readonly HttpClient _client = new();

    private const string Url = "https://sign.lagrangecore.org/api/sign";

    private readonly Timer _timer;

    public LinuxSigner()
    {
        _timer = new Timer(_ =>
        {
            bool reconnect = Available = Test();
            if (reconnect) _timer?.Change(-1, 5000);
        });
    }
    
    public override byte[]? Sign(string cmd, uint seq, byte[] body, out byte[]? ver, out string? token)
    {
        ver = null;
        token = null;
        if (!WhiteListCommand.Contains(cmd)) return null;
        if (!Available || string.IsNullOrEmpty(Url)) return new byte[35]; // Dummy signature
        
        var payload = new JsonObject
        {
            { "cmd", cmd },
            { "seq", seq },
            { "src", body.Hex() },
        };

        try
        {
            var message = _client.PostAsJsonAsync(Url, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            ver = json?["value"]?["extra"]?.ToString().UnHex() ?? Array.Empty<byte>();
            token = Encoding.ASCII.GetString(json?["value"]?["token"]?.ToString().UnHex() ?? Array.Empty<byte>());
            return json?["value"]?["sign"]?.ToString().UnHex() ?? new byte[20];
        }
        catch (Exception)
        {
            Available = false;
            _timer.Change(0, 5000);
            
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nameof(LinuxSigner)}] Failed to get signature, using dummy signature");
            return new byte[20]; // Dummy signature
        }
    }

    public override bool Test()
    {
        try
        {
            string response = Http.GetAsync($"{Url}/ping").GetAwaiter().GetResult();
            if (JsonSerializer.Deserialize<JsonObject>(response)?["code"]?.GetValue<int>() == 0) return true;
        }
        catch
        {
            return false;
        }

        return false;
    }
}