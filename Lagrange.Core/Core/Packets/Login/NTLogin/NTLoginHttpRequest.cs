using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Login.NTLogin;

public class NTLoginHttpRequest
{
    [JsonPropertyName("appid")] public long Appid { get; set; }

    [JsonPropertyName("faceUpdateTime")] public long FaceUpdateTime { get; set; }

    [JsonPropertyName("qrsig")] public string Qrsig { get; set; }
}