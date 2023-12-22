using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotPrivateMsg(uint selfId) : OneBotEntityBase(selfId, "message")
{
    [JsonPropertyName("message_type")] public string MessageType { get; } = "private";

    [JsonPropertyName("sub_type")] public string SubType { get; } = "friend";

    [JsonPropertyName("message_id")] public uint MessageId { get; set; }
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("message")] public List<OneBotSegment> Message { get; set; } = new();

    [JsonPropertyName("raw_message")] public string RawMessage { get; set; } = string.Empty;

    [JsonPropertyName("font")] public int Font { get; set; } = 0;

    [JsonPropertyName("sender")] public OneBotSender GroupSender { get; set; } = new();
}