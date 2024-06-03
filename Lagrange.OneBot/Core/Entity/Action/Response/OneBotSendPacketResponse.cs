using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotSendPacketResponse
{
    [JsonPropertyName("sequence")] public int Sequence { get; set; }
    
    [JsonPropertyName("result")] public string Result { get; set; } = string.Empty;
    
    [JsonPropertyName("retcode")] public int RetCode { get; set; }
    
    [JsonPropertyName("extra")] public string? Extra { get; set; }
}