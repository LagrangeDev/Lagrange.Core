using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotFriend
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("q_id")] public string? QId { get; set; }

    [JsonPropertyName("nickname")] public string NickName { get; set; } = "";

    [JsonPropertyName("remark")] public string Remark { get; set; } = "";
}