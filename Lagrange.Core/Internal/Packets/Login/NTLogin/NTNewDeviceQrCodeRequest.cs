using System.Text.Json.Serialization;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[Serializable]
internal class NTNewDeviceQrCodeRequest
{
    [JsonPropertyName("str_dev_auth_token")] public string StrDevAuthToken { get; set; }

    [JsonPropertyName("uint32_flag")] public long Uint32Flag { get; set; }

    [JsonPropertyName("uint32_url_type")] public long Uint32UrlType { get; set; }

    [JsonPropertyName("str_uin_token")] public string StrUinToken { get; set; }

    [JsonPropertyName("str_dev_type")] public string StrDevType { get; set; }

    [JsonPropertyName("str_dev_name")] public string StrDevName { get; set; }
}