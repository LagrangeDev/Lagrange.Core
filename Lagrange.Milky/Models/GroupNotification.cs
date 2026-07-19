using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(JoinRequestGroupNotification), "join_request")]
[JsonDerivedType(typeof(AdminChangeGroupNotification), "admin_change")]
[JsonDerivedType(typeof(KickGroupNotification), "kick")]
[JsonDerivedType(typeof(QuitGroupNotification), "quit")]
[JsonDerivedType(typeof(InvitedJoinRequestGroupNotification), "invited_join_request")]
public abstract class GroupNotificationBase;

public sealed class JoinRequestGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("is_filtered")] public required bool IsFiltered { get; init; }
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("state")] public required string State { get; init; }
    [JsonPropertyName("operator_id")] public required long? OperatorId { get; init; }
    [JsonPropertyName("comment")] public required string Comment { get; init; }
}

public sealed class AdminChangeGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    [JsonPropertyName("is_set")] public required bool IsSet { get; init; }
    [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
}

public sealed class KickGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
}

public sealed class QuitGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
}

public sealed class InvitedJoinRequestGroupNotification : GroupNotificationBase
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    [JsonPropertyName("state")] public required string State { get; init; }
    [JsonPropertyName("operator_id")] public required long? OperatorId { get; init; }
}