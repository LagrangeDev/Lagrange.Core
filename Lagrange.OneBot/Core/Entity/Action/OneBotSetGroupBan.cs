using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupBan
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("duration")] public uint Duration { get; set; }
}