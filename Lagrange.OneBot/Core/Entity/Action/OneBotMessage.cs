using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotMessage
{
    [JsonPropertyName("message_type")] public string MessageType { get; set; } = "";
    
    [JsonPropertyName("user_id")] public uint? UserId { get; set; }
    
    [JsonPropertyName("group_id")] public uint? GroupId { get; set; }

    [JsonPropertyName("message")] public List<OneBotSegment> Messages { get; set; } = new();
}