using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotGetMessageResponse(DateTime time, string messageType, int messageId, OneBotSender sender, List<OneBotSegment> message)
{
    [JsonPropertyName("time")] public int Time { get; set; } = (int)(TimeZoneInfo.ConvertTimeToUtc(time, TimeZoneInfo.Local) - DateTime.UnixEpoch).TotalSeconds;
    
    [JsonPropertyName("message_type")] public string MessageType { get; set; } = messageType;

    [JsonPropertyName("message_id")] public int MessageId { get; set; } = messageId;

    [JsonPropertyName("real_id")] public int RealId { get; set; } = messageId;
    
    [JsonPropertyName("sender")] public OneBotSender Sender { get; set; } = sender;

    [JsonPropertyName("message")] public List<OneBotSegment> Message { get; set; } = message;
}