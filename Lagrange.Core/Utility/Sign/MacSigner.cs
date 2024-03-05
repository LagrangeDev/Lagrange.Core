using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Generator;
using Lagrange.Core.Utility.Network;
using ProtoBuf;

namespace Lagrange.Core.Utility.Sign;

internal class MacSigner : SignProvider
{
    private readonly HttpClient _client = new();
    
    private const string MacOsUrl = "http://127.0.0.1:7458/api/sign";
    
    public override byte[] Sign(BotDeviceInfo device, BotKeystore keystore, string cmd, uint seq, byte[] body)
    {
        if (!WhiteListCommand.Contains(cmd)) return Array.Empty<byte>();
        if (!Available || string.IsNullOrEmpty(MacOsUrl)) return Array.Empty<byte>(); // Dummy signature

        try
        {
            var payload = new JsonObject
            {
                { "cmd", cmd },
                { "seq", seq },
                { "src", body.Hex() },
            };
            var message = _client.PostAsJsonAsync(MacOsUrl, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            var secSig = json?["value"]?["sign"]?.ToString().UnHex();
            var secDeviceToken = json?["value"]?["token"]?.ToString().UnHex();
            var secExtra = json?["value"]?["extra"]?.ToString().UnHex();

            var signature = new ReserveFields
            {
                TraceParent = StringGen.GenerateTrace(),
                Uid = keystore.Uid,
                TransInfo = new()
                {
                    { "client_conn_seq" ,  "1709470839"}
                },
                SecInfo = new()
                {
                    SecSig = secSig,
                    SecDeviceToken = Encoding.UTF8.GetString(secDeviceToken),
                    SecExtra = secExtra,
                },
                NtCoreVersion = 101
            };
            var stream = new MemoryStream();
            Serializer.Serialize(stream, signature);
            return stream.ToArray();
        }
        catch (Exception)
        {
            Available = false;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nameof(MacSigner)}] Failed to get signature, using dummy signature");
            
            return Array.Empty<byte>(); // Dummy signature
        }
    }

    public override bool Test()
    {
        throw new NotImplementedException();
    }
}