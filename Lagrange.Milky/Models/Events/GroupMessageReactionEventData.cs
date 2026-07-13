using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class GroupMessageReactionEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
    [JsonPropertyName("face_id")] public required string FaceId { get; init; }
    [JsonPropertyName("reaction_type")] public required string ReactionType { get; init; }
    [JsonPropertyName("is_add")] public required bool IsAdd { get; init; }
}