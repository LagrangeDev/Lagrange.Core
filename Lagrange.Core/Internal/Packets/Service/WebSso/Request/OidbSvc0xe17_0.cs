using System.Text.Json.Serialization;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.WebSso.Request;

// ref https://github.com/Mrs4s/MiraiGo/blob/54bdd873e3fed9fe1c944918924674dacec5ac76/client/web.go#L23
[Serializable]
[WebSso("ti.qq.com", "OidbSvc.0xe17_0")]
internal class OidbSvc0xe17_0
{
    [JsonPropertyName("uint64_uin")] public uint Uin { get; set; }

    [JsonPropertyName("uint64_top")] public uint Top { get; set; } = 0;

    [JsonPropertyName("uint32_req_num")] public uint ReqNum { get; set; } = 99;

    [JsonPropertyName("bytes_cookies")] public string Cookies { get; set; } = string.Empty;
}