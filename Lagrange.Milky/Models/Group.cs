using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models;

public class Group
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("group_name")] public required string GroupName { get; init; }
    [JsonPropertyName("member_count")] public required long MemberCount { get; init; }
    [JsonPropertyName("max_member_count")] public required long MaxMemberCount { get; init; }
    [JsonPropertyName("remark")] public required string Remark { get; init; }
    [JsonPropertyName("created_time")] public required long CreatedTime { get; init; }
    [JsonPropertyName("description")] public required string Description { get; init; }
    [JsonPropertyName("question")] public required string Question { get; init; }
    [JsonPropertyName("announcement")] public required string Announcement { get; init; }
}