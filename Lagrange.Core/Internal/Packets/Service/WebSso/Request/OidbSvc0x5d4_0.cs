using System.Text.Json.Serialization;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.WebSso.Request;

// ref https://github.com/Mrs4s/MiraiGo/blob/54bdd873e3fed9fe1c944918924674dacec5ac76/client/web.go#L23
[Serializable]
[WebSso("ti.qq.com", "OidbSvc.0x5d4_0")]
internal class OidbSvc0x5d4_0
{
    [JsonPropertyName("uin_list")] public uint[] UinList { get; set; }
}