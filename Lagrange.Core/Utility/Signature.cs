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
    
    private static readonly string[] WhiteListCommand =
    {
        "trpc.o3.ecdh_access.EcdhAccess.SsoEstablishShareKey",
        "trpc.o3.ecdh_access.EcdhAccess.SsoSecureAccess",
        "trpc.o3.report.Report.SsoReport",
        "MessageSvc.PbSendMsg",
        "wtlogin.trans_emp",
        "wtlogin.login",
        "trpc.login.ecdh.EcdhService.SsoKeyExchange",
        "trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLogin",
        "trpc.login.ecdh.EcdhService.SsoNTLoginEasyLogin",
        "trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLoginNewDevice",
        "trpc.login.ecdh.EcdhService.SsoNTLoginEasyLoginUnusualDevice",
        "trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLoginUnusualDevice",
        "OidbSvcTrpcTcp.0x11ec_1",
        "OidbSvcTrpcTcp.0x758_1",
        "OidbSvcTrpcTcp.0x7c2_5",
        "OidbSvcTrpcTcp.0x10db_1",
        "OidbSvcTrpcTcp.0x8a1_7",
        "OidbSvcTrpcTcp.0x89a_0",
        "OidbSvcTrpcTcp.0x89a_15",
        "OidbSvcTrpcTcp.0x88d_0",
        "OidbSvcTrpcTcp.0x88d_14",
        "OidbSvcTrpcTcp.0x112a_1",
        "OidbSvcTrpcTcp.0x587_74",
        "OidbSvcTrpcTcp.0x1100_1",
        "OidbSvcTrpcTcp.0x1102_1",
        "OidbSvcTrpcTcp.0x1103_1",
        "OidbSvcTrpcTcp.0x1107_1",
        "OidbSvcTrpcTcp.0x1105_1",
        "OidbSvcTrpcTcp.0xf88_1",
        "OidbSvcTrpcTcp.0xf89_1",
        "OidbSvcTrpcTcp.0xf57_1",
        "OidbSvcTrpcTcp.0xf57_106",
        "OidbSvcTrpcTcp.0xf57_9",
        "OidbSvcTrpcTcp.0xf55_1",
        "OidbSvcTrpcTcp.0xf67_1",
        "OidbSvcTrpcTcp.0xf67_5"
    };

    /// <summary>
    /// Get O3Signature
    /// </summary>
    public static byte[]? GetSignature(string cmd, uint seq, byte[] body)
    {
        if (!WhiteListCommand.Contains(cmd)) return null;
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