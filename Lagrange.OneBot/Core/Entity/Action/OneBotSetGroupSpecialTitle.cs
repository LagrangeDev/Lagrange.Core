using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupSpecialTitle
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("special_title")] public string SpecialTitle { get; set; } = string.Empty;
    
    [JsonPropertyName("duration")] public int Duration { get; set; }
}