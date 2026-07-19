using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models;

public abstract class GroupNotificationBase
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
}

public sealed class JoinRequestGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("is_filtered")] public required bool IsFiltered { get; init; }
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("state")] public required string State { get; init; }
    [JsonPropertyName("operator_id")] public required long? OperatorId { get; init; }
    [JsonPropertyName("comment")] public required string Comment { get; init; }
}

public sealed class AdminChangeGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    [JsonPropertyName("is_set")] public required bool IsSet { get; init; }
    [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
}

public sealed class KickGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
}

public sealed class QuitGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
}

public sealed class InvitedJoinRequestGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    [JsonPropertyName("state")] public required string State { get; init; }
    [JsonPropertyName("operator_id")] public required long? OperatorId { get; init; }
}