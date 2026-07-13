using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class FriendRequestEventData
{
    [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
    [JsonPropertyName("initiator_uid")] public required string InitiatorUid { get; init; }
    [JsonPropertyName("comment")] public required string Comment { get; init; }
    [JsonPropertyName("via")] public required string Via { get; init; }
}