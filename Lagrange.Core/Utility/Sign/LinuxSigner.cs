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
        if (!WhiteListCommand.Contains(cmd)) return Array.Empty<byte>();
        if (!Available || string.IsNullOrEmpty(Url)) return Array.Empty<byte>(); // Dummy signature

        try
        {
            var payload = new JsonObject
            {
                { "cmd", cmd },
                { "seq", seq },
                { "src", body.Hex() },
            };
            var message = _client.PostAsJsonAsync(Url, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<JsonObject>(response);

            var secSig = json?["value"]?["sign"]?.ToString().UnHex();

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
                    SecSig = secSig
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
            _timer.Change(0, 5000);

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nameof(LinuxSigner)}] Failed to get signature, using dummy signature");
            return Array.Empty<byte>(); // Dummy signature
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