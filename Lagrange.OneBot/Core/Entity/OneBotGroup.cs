using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotGroup(BotGroup group)
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = group.GroupUin;

    [JsonPropertyName("group_name")] public string GroupName { get; set; } = group.GroupName;

    [JsonPropertyName("member_count")] public uint MemberCount { get; set; } = group.MemberCount;

    [JsonPropertyName("max_member_count")] public uint MaxMemberCount { get; set; } = group.MaxMember;
}