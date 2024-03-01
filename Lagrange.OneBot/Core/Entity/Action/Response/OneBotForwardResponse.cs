using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotForwardResponse(int messageId, string forwardId)
{
    [JsonPropertyName("message_id")] public int MessageId { get; set; } = messageId;

    [JsonPropertyName("forward_id")] public string ForwardId { get; set; } = forwardId;
}