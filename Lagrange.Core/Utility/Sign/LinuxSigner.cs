using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;

namespace Lagrange.Core.Utility.Sign;

internal class LinuxSigner : SignProvider
{
    private const string Url = "";

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