using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;

namespace Lagrange.Core.Utility.Sign;

internal class LinuxSigner : SignProvider
{
    private const string Url = "";
    
    public override byte[]? Sign(string cmd, uint seq, byte[] body, out byte[]? ver, out string? token)
    {
        ver = null;
        token = null;
        if (!WhiteListCommand.Contains(cmd)) return null;
        if (!Available || string.IsNullOrEmpty(Url)) return new byte[20]; // Dummy signature

        try
        {
            var payload = new Dictionary<string, string>
            {
                { "cmd", cmd },
                { "seq", seq.ToString() },
                { "src", body.Hex() },
            };
            string response = Http.GetAsync(Url, payload).GetAwaiter().GetResult();
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            return json?["value"]?["sign"]?.ToString().UnHex() ?? new byte[20];
        }
        catch (Exception)
        {
            Available = false;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nameof(LinuxSigner)}] Failed to get signature, using dummy signature");
            return new byte[20]; // Dummy signature
        }
    }
}