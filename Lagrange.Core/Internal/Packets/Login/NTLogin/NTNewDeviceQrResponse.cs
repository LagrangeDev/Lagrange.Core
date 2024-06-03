using System.Text.Json.Serialization;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin;

#pragma warning disable CS8618

[Serializable]
public class NTNewDeviceQrCodeResponse
{
    [JsonPropertyName("uint32_guarantee_status")] public long Uint32GuaranteeStatus { get; set; }

    [JsonPropertyName("str_url")] public string StrUrl { get; set; }

    [JsonPropertyName("ActionStatus")] public string ActionStatus { get; set; }
    
    [JsonPropertyName("str_nt_succ_token")] public string? StrNtSuccToken { get; set; }
    
    [JsonPropertyName("ErrorCode")] public long ErrorCode { get; set; }

    [JsonPropertyName("ErrorInfo")] public string ErrorInfo { get; set; }
}