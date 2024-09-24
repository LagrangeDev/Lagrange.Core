using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotEssenceMessage
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
}

[Serializable]
public class OneBotEssenceMessageSegment
{
    [JsonPropertyName("sender_id")] public uint SenderId { get; set; }

    [JsonPropertyName("sender_nick")] public string SenderNick { get; set; } = string.Empty;
    
    [JsonPropertyName("sender_time")] public uint SenderTime { get; set; }
    
    [JsonPropertyName("operator_id")] public uint OperatorId { get; set; }

    [JsonPropertyName("operator_nick")] public string OperatorNick { get; set; } = string.Empty;

    [JsonPropertyName("operator_time")] public uint OperatorTime { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }

    [JsonPropertyName("content")] public List<OneBotSegment> Content { get; set; } = [];
}