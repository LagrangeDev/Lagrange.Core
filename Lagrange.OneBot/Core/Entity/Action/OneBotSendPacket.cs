using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSendPacket
{
    [JsonPropertyName("data")] public string Data { get; set; } = string.Empty;
    
    [JsonPropertyName("command")] public string Command { get; set; } = string.Empty;
    
    [JsonPropertyName("sign")] public bool Sign { get; set; }

    [JsonPropertyName("type")] public byte Type { get; set; } = 12;
}