using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class GroupMemberIncreaseEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("operator_id")] public required long? OperatorId { get; init; }
    [JsonPropertyName("invitor_id")] public required long? InvitorId { get; init; }
}