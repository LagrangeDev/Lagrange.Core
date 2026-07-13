using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class MessageRecallEventData
{
    [JsonPropertyName("message_scene")] public required string MessageScene { get; init; }
    [JsonPropertyName("peer_id")] public required long PeerId { get; init; }
    [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
    [JsonPropertyName("sender_id")] public required long SenderId { get; init; }
    [JsonPropertyName("operator_id")] public required long OperatorId { get; init; }
    [JsonPropertyName("display_suffix")] public required string DisplaySuffix { get; init; }
}