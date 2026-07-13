using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class GroupInvitedJoinRequestEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
}