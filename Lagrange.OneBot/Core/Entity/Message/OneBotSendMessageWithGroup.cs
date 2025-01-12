using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotSendMessageWithGroup
{
    [JsonPropertyName("message_id")] public int MessageId { get; set; }
    
    [JsonPropertyName("target_group_id")] public uint TargetGroupId { get; set; }
}