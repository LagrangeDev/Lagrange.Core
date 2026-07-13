using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

// TODO: group mute event is not implemented in core
public class GroupMuteEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
    [JsonPropertyName("duration")] public required int Duration { get; init; }
}