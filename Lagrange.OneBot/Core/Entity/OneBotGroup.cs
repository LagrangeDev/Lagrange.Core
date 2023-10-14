using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotGroup(uint groupId, string groupName)
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;

    [JsonPropertyName("group_name")] public string GroupName { get; set; } = groupName;

    [JsonPropertyName("member_count")] public int MemberCount { get; set; }
    
    [JsonPropertyName("max_member_count")] public int MaxMemberCount { get; set; }
}