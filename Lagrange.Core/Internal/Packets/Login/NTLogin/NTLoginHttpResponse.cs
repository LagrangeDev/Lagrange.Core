using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Login.NTLogin;

public class NTLoginHttpResponse
{
    [JsonPropertyName("retCode")] public int RetCode { get; set; }

    [JsonPropertyName("errMsg")] public string ErrMsg { get; set; }

    [JsonPropertyName("qrSig")] public string QrSig { get; set; }

    [JsonPropertyName("uin")] public uint Uin { get; set; }

    [JsonPropertyName("faceUrl")] public string FaceUrl { get; set; }

    [JsonPropertyName("faceUpdateTime")] public long FaceUpdateTime { get; set; }
}