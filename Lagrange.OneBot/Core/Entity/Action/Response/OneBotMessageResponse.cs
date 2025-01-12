using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotMessageResponse(int messageId, int messageSeq = 0)
{
    [JsonPropertyName("message_id")] public int MessageId { get; set; } = messageId;
    [JsonPropertyName("message_seq")] public int MessageSeq { get; set; } = messageSeq;
}