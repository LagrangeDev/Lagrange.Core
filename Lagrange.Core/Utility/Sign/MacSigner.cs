using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Utility.Sign;

internal class MacSigner : SignProvider
{
    private readonly HttpClient _client = new();
    
    private const string MacOsUrl = "http://127.0.0.1:7458/api/sign";
    
    public override byte[]? Sign(string cmd, uint seq, byte[] body, out byte[]? ver, out string? token)
    {
        ver = null;
        token = null;
        if (!WhiteListCommand.Contains(cmd)) return null;
        if (!Available || string.IsNullOrEmpty(MacOsUrl)) return new byte[35]; // Dummy signature
        
        var payload = new JsonObject
        {
            { "cmd", cmd },
            { "seq", seq },
            { "src", body.Hex() },
        };

        try
        {
            var message = _client.PostAsJsonAsync(MacOsUrl, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            ver = json?["value"]?["extra"]?.ToString().UnHex() ?? Array.Empty<byte>();
            token = Encoding.ASCII.GetString(json?["value"]?["token"]?.ToString().UnHex() ?? Array.Empty<byte>());
            return json?["value"]?["sign"]?.ToString().UnHex() ?? new byte[35];
        }
        catch (Exception)
        {
            Available = false;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nameof(MacSigner)}] Failed to get signature, using dummy signature");
            
            return new byte[35]; // Dummy signature
        }
    }

    public override bool Test()
    {
        throw new NotImplementedException();
    }
}