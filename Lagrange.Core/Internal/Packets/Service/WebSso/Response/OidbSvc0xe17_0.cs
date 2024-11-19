using System.Text;
using System.Text.Json.Serialization;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.WebSso.Response;

// ref https://github.com/Mrs4s/MiraiGo/blob/54bdd873e3fed9fe1c944918924674dacec5ac76/client/web.go
[Serializable]
internal class OidbSvc0xe17_0
{
    [JsonPropertyName("rpt_block_list")] public BlockList BlockList { get; set; }

    [JsonPropertyName("ErrorCode")] public int ErrorCode { get; set; }
}

[Serializable]
internal class BlockList
{
    [JsonPropertyName("uint64_uin")] public uint Uin { get; set; }

    [JsonPropertyName("str_uid")] public string Uid { get; set; }

    [JsonPropertyName("bytes_nick")] public string _nickname { get; set; }

    public string Nickname =>
        string.IsNullOrEmpty(_nickname)
            ? string.Empty
            : Encoding.UTF8.GetString(Convert.FromBase64String(_nickname));

    [JsonPropertyName("uint32_age")] public uint Age { get; set; }

    [JsonPropertyName("uint32_sex")] public uint Sex { get; set; }

    [JsonPropertyName("bytes_source")] public string _source { get; set; }

    public string Source => string.IsNullOrEmpty(_source)
        ? string.Empty
        : Encoding.UTF8.GetString(Convert.FromBase64String(_source));
}