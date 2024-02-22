using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupRecall(uint selfId) : OneBotNotify(selfId, "group_recall")
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }
    
    [JsonPropertyName("operator_id")] public uint OperatorId { get; set; }
}