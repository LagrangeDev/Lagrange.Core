using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupMsgHistory
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }
    
    [JsonPropertyName("count")] public int Count { get; set; } = 20;
}