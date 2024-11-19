using System.Text.Json.Serialization;

namespace Lagrange.Core.Internal.Packets.Service.WebSso.Response;

[Serializable]
internal class OidbSvc0x5d4_0
{
    [JsonPropertyName("ErrorCode")] public int ErrorCode { get; set; }
}