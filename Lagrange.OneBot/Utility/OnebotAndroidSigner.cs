using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;
using ProtoBuf;
using Lagrange.Core.Common;
using Lagrange.Core.Utility.Generator;
using Microsoft.Extensions.Configuration;

namespace Lagrange.Core.Utility.Sign;

internal class OnebotAndroidSigner : SignProvider
{
    private readonly string Url;
    private readonly string SignUrl;
    private readonly string EnergyUrl;
    private readonly string GetXwDebugIdUrl;
    private readonly string TestUrl;

    private readonly HttpClient _client = new();
    private readonly Timer _timer;

    public OnebotAndroidSigner(IConfiguration config)
    {
        Url = config["AndroidSignServerUrl"] ?? "";
        SignUrl = $"{Url}/sign";
        EnergyUrl = $"{Url}/energy";
        GetXwDebugIdUrl = $"{Url}/get_xw_debug_id";
        TestUrl = $"{Url}/ping";
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

        try
        {
            var payload = new JsonObject
            {
                { "uin", keystore.Uin },
                { "cmd", cmd },
                { "seq", seq },
                { "buffer", body.Hex() },
                { "android_id", device.System.AndroidId },
                { "guid", device.System.Guid.ToByteArray().Hex() }
            };

            var message = _client.PostAsJsonAsync(SignUrl, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            var secSig = json?["data"]?["sign"]?.ToString().UnHex();
            var secDeviceToken = json?["data"]?["token"]?.ToString().UnHex();
            var secExtra = json?["data"]?["extra"]?.ToString().UnHex();

            signature = new ReserveFields
            {
                Flag = 1,
                LocaleId = 2052,
                Qimei = keystore.Session.QImei?.Q36,
                NewconnFlag = 0,
                TraceParent = StringGen.GenerateTrace(),
                Uid = keystore.Uid,
                Imsi = 0,
                NetworkType = 1,
                IpStackType = 1,
                MsgType = 0,
                SecInfo = new()
                {
                    SecSig = secSig,
                    SecDeviceToken = Encoding.UTF8.GetString(secDeviceToken),
                    SecExtra = secExtra,
                },
                NtCoreVersion = 100,
                SsoIpOrigin = 3
            };
            stream = new MemoryStream();
            Serializer.Serialize(stream, signature);
            return stream.ToArray();
        }
        catch (Exception)
        {
            Available = false;
            _timer.Change(0, 5000);

            return stream.ToArray();
        }
    }

    public override byte[] Energy(string sdkVersion, uint uin, Guid guid, string data)
    {
        try
        {
            var payload = new Dictionary<string, string>
            {
                { "version", sdkVersion },
                { "uin", uin.ToString() },
                { "guid", guid.ToByteArray().Hex() },
                { "data", data },
            };
            string response = Http.GetAsync(EnergyUrl, payload).GetAwaiter().GetResult();
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            return json?["data"].ToString().UnHex();
        }
        catch (Exception)
        {
            return Array.Empty<byte>();
        }
    }

    public override byte[] GetXwDebugId(uint uin, string data)
    {
        try
        {
            var payload = new Dictionary<string, string>
            {
                { "uin", uin.ToString() },
                { "data", data },
            };
            string response = Http.GetAsync(GetXwDebugIdUrl, payload).GetAwaiter().GetResult();
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            return json?["data"].ToString().UnHex();
        }
        catch (Exception)
        {
            return Array.Empty<byte>();
        }
    }

    public override bool Test()
    {
        try
        {
            string response = Http.GetAsync(TestUrl).GetAwaiter().GetResult();
            if (JsonSerializer.Deserialize<JsonObject>(response)?["code"]?.GetValue<int>() == 0)
                return true;
        }
        catch
        {
            return false;
        }

        return false;
    }
}