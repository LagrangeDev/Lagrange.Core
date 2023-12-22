using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotGetMessageResponse(DateTime time, string messageType, uint messageId, List<OneBotSegment> message)
{
    [JsonPropertyName("time")] public int Time { get; set; } = (int)(time - DateTime.UnixEpoch).TotalSeconds;
    
    [JsonPropertyName("message_type")] public string MessageType { get; set; } = messageType;

    [JsonPropertyName("message_id")] public uint MessageId { get; set; } = messageId;

    [JsonPropertyName("real_id")] public uint RealId { get; set; } = messageId;
    
    [JsonPropertyName("sender")] public OneBotSender Sender { get; set; } = new();
    
    [JsonPropertyName("message")] public List<OneBotSegment> Message { get; set; } = message;
}