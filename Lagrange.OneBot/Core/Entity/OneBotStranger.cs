using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotStranger
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("nickname")] public string NickName { get; set; } = string.Empty;

    [JsonPropertyName("sex")] public string Sex { get; set; } = "unknown";
    
    [JsonPropertyName("age")] public uint Age { get; set; }
}