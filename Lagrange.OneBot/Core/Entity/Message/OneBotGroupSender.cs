using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupSender(uint userId, string nickname, string card, int level, GroupMemberPermission permission)
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("nickname")] public string Nickname { get; set; } = nickname;

    [JsonPropertyName("card")] public string Card { get; set; } = card;

    [JsonPropertyName("sex")] public string Sex { get; set; } = "unknown";

    [JsonPropertyName("age")] public uint Age { get; set; } = 0;

    [JsonPropertyName("area")] public string Area { get; set; } = string.Empty;

    [JsonPropertyName("level")] public string Level { get; set; } = level.ToString();

    [JsonPropertyName("role")] public string Role { get; set; } = permission switch
    {
        GroupMemberPermission.Owner => "owner",
        GroupMemberPermission.Admin => "admin",
        GroupMemberPermission.Member => "member",
        _ => "unknown"
    };

    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
}