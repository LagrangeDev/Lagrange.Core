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

internal class LinuxSigner : SignProvider
{
    private const string Url = "";
    private const string SignUrl = $"{Url}/sign";
    private const string TestUrl = $"{Url}/ping";

    private readonly HttpClient _client = new();
    private readonly Timer _timer;

    public LinuxSigner()
    {
        _timer = new Timer(_ =>
        {
            bool reconnect = Available = Test();
            if (reconnect) _timer?.Change(-1, 5000);
        });
    }

    public override byte[] Sign(BotDeviceInfo device, BotKeystore keystore, string cmd, uint seq, byte[] body)
    {
        var signature = new ReserveFields
        {
            TraceParent = StringGen.GenerateTrace(),
            Uid = keystore.Uid
        };
        var stream = new MemoryStream();
        Serializer.Serialize(stream, signature);
        if (!WhiteListCommand.Contains(cmd)) return stream.ToArray();
        if (!Available || string.IsNullOrEmpty(Url)) return stream.ToArray();

        try
        {
            var payload = new JsonObject
            {
                { "cmd", cmd },
                { "seq", seq },
                { "src", body.Hex() },
            };
            
            var message = _client.PostAsJsonAsync(SignUrl, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            var secSig = json?["value"]?["sign"]?.ToString().UnHex();
            var secDeviceToken = json?["value"]?["token"]?.ToString().UnHex();
            var secExtra = json?["value"]?["extra"]?.ToString().UnHex();

            signature.SecInfo = new()
            {
                SecSig = secSig,
                SecDeviceToken = secDeviceToken == null ? null : Encoding.UTF8.GetString(secDeviceToken),
                SecExtra = secExtra,
            };
            stream = new MemoryStream();
            Serializer.Serialize(stream, signature);
            return stream.ToArray();
        }
        catch (Exception)
        {
            Available = false;
            _timer.Change(0, 5000);

            return stream.ToArray(); // Dummy signature
        }
    }

    public override bool Test()
    {
        try
        {
            string response = Http.GetAsync(TestUrl).GetAwaiter().GetResult();
            if (JsonSerializer.Deserialize<JsonObject>(response)?["code"]?.GetValue<int>() == 0) return true;
        }
        catch
        {
            return false;
        }

        return false;
    }
}