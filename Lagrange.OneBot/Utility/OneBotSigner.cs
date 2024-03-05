using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Network;
using Lagrange.Core.Utility.Sign;
using Microsoft.Extensions.Configuration;
using ProtoBuf;

namespace Lagrange.OneBot.Utility;

public class OneBotSigner : SignProvider
{
    private readonly string _signServer;
    private readonly string _signUrl;
    private readonly string _energyUrl;
    private readonly string _getXwDebugIdUrl;

    private readonly Timer _timer;

    private readonly HttpClient _client;

    private readonly HttpClient _client;

    public OneBotSigner(IConfiguration config)
    {
        _signServer = config["SignServerUrl"] ?? "";
        _signUrl = $"{_signServer}/sign";
        _energyUrl = $"{_signServer}/energy";
        _getXwDebugIdUrl = $"{_signServer}/get_xw_debug_id";
/*
        _timer = new Timer(_ =>
        {
            bool reconnect = Available = Test();
            if (reconnect) _timer?.Change(-1, 5000);
        });*/
    }

    public override byte[] Sign(BotDeviceInfo device, BotKeystore keystore, string cmd, uint seq, byte[] body)
    {
        if (!WhiteListCommand.Contains(cmd)) return Array.Empty<byte>();
        // if (!Available || string.IsNullOrEmpty(_signUrl)) return Array.Empty<byte>();


        try
        {
            var payload = new JsonObject
            {
                { "uin", keystore.Uin },
                { "cmd", cmd },
                { "seq", seq },
                { "src", body.Hex() },
            };
            
            var message = _client.PostAsJsonAsync(_signServer, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            var secSig = json?["data"]?["sign"]?.ToString().UnHex();
            var secDeviceToken = json?["data"]?["token"]?.ToString().UnHex();
            var secExtra = json?["data"]?["extra"]?.ToString().UnHex();

            var signature = new ReserveFields
            {
                Flag = 1,
                LocaleId = 2052,
                Qimei = keystore.Session.QImei?.Q36,
                NewconnFlag = 0,
                TraceParent = "",
                Uid = keystore.Uid,
                Imsi = 0,
                NetworkType = 1,
                IpStackType = 1,
                MsgType = 0,
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
                NtCoreVersion = 100,
                SsoIpOrigin = 3
            };
            var stream = new MemoryStream();
            Serializer.Serialize(stream, signature);
            return stream.ToArray();
        }
        catch
        {
            Available = false;
            _timer.Change(0, 5000);

            return Array.Empty<byte>();
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
            string response = Http.GetAsync(_energyUrl, payload).GetAwaiter().GetResult();
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
            string response = Http.GetAsync(_getXwDebugIdUrl, payload).GetAwaiter().GetResult();
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
            string response = Http.GetAsync($"{_signServer}/ping").GetAwaiter().GetResult();
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