using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class GroupInvitationEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("invitation_seq")] public required long InvitationSeq { get; init; }
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("source_group_id")] public required long? TargetUserId { get; init; }
}