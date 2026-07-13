using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class GroupJoinRequestEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("is_filtered")] public required bool IsFiltered { get; init; }
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("comment")] public required string Comment { get; init; }
}