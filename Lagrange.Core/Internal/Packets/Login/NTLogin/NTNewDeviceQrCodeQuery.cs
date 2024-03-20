using System.Text.Json.Serialization;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[Serializable]
internal class NTNewDeviceQrCodeQuery
{
    [JsonPropertyName("uint32_flag")] public long Uint32Flag { get; set; }
    
    [JsonPropertyName("bytes_token")] public string Token { get; set; }
}