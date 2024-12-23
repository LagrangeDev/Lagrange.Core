using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotStranger
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("avatar")] public string Avatar { get; set; } = string.Empty;

    [JsonPropertyName("q_id")] public string? QId { get; set; }

    [JsonPropertyName("nickname")] public string NickName { get; set; } = string.Empty;

    [JsonPropertyName("sign")] public string Sign { get; set; } = string.Empty;

    [JsonPropertyName("sex")] public string Sex { get; set; } = "unknown";

    [JsonPropertyName("age")] public uint Age { get; set; }

    [JsonPropertyName("level")] public uint Level { get; set; }

    [JsonPropertyName("status")] public OneBotFriendStatus Status { get; set; } = new();

    [JsonPropertyName("RegisterTime")] public DateTime RegisterTime { get; set; }

    [JsonPropertyName("Business")] public List<OneBotBusiness> Business { get; set; } = new();
}