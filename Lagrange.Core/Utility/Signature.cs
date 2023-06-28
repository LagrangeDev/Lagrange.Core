using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;

#pragma warning disable CS8618

namespace Lagrange.Core.Utility;

internal static class Signature
{
    private const string Url = "";

    private const string Key = "";
    
    private static bool _available = true;

    /// <summary>
    /// Get O3Signature
    /// </summary>
    public static byte[] GetSignature(string cmd, uint seq, byte[] body)
    {
        if (!_available || string.IsNullOrEmpty(Url)) return new byte[20]; // Dummy signature
        
        try
        {
            var payload = new Dictionary<string, string>
            {
                { "cmd", cmd },
                { "seq", seq.ToString() },
                { "src", body.Hex() },
                { "key", Key }
            };
            string response = Http.GetAsync(Url, payload).GetAwaiter().GetResult();
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            return json?["value"]?["sign"]?.ToJsonString()[1..^1].UnHex() ?? new byte[20];
        }
        catch (Exception)
        {
            _available = false;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nameof(Signature)}] Failed to get signature, using dummy signature");
            return new byte[20]; // Dummy signature
        }
    }
}