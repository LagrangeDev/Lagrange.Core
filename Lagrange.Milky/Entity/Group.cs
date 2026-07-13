using System.Text.Json.Serialization;

namespace Lagrange.Milky.Entity;

public class Group(long groupId, string groupName, long memberCount, long maxMemberCount, string? remark, long createdTime, string? description, string? question, string? announcement)
{
    [JsonPropertyName("group_id")]
    public long GroupId { get; } = groupId;

    [JsonPropertyName("group_name")]
    public string GroupName { get; } = groupName;

    [JsonPropertyName("member_count")]
    public long MemberCount { get; } = memberCount;

    [JsonPropertyName("max_member_count")]
    public long MaxMemberCount { get; } = maxMemberCount;

    [JsonPropertyName("remark")]
    public string? Remark { get; } = remark;

    [JsonPropertyName("created_time")]
    public long CreatedTime { get; } = createdTime;

    [JsonPropertyName("description")]
    public string? Description { get; } = description;

    [JsonPropertyName("question")]
    public string? Question { get; } = question;

    [JsonPropertyName("announcement")]
    public string? Announcement { get; } = announcement;
}
