using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotGroupMember(uint groupId, uint uin, string permission, string groupLevel, string? memberCard,
    string memberName, uint joinTime, uint lastMsgTime)
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;

    [JsonPropertyName("user_id")] public uint Uin { get; set; } = uin;

    [JsonPropertyName("nickname")] public string MemberName { get; set; } = memberName;

    [JsonPropertyName("card")] public string? MemberCard { get; set; } = memberCard;

    [JsonPropertyName("sex")] public string Sex { get; set; } = "";

    [JsonPropertyName("age")] public int Age { get; set; } = 0;

    [JsonPropertyName("area")] public string Area { get; set; } = "";

    [JsonPropertyName("join_time")] public uint JoinTime { get; set; } = joinTime;

    [JsonPropertyName("last_sent_time")] public uint LastMsgTime { get; set; } = lastMsgTime;

    [JsonPropertyName("level")] public string GroupLevel { get; set; } = groupLevel;

    [JsonPropertyName("role")] public string Permission { get; set; } = permission;

    [JsonPropertyName("unfriendly")] public bool Unfriendly { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; } = "";

    [JsonPropertyName("title_expire_time")] public uint TitleExpireTime { get; set; } = 0;

    [JsonPropertyName("card_changeable")] public bool CardChangeable { get; set; }
}