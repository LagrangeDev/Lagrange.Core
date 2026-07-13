using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class MentionIncomingSegment : IncomingSegmentBase<MentionIncomingSegmentData>;
public class MentionIncomingSegmentData
{
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("name")] public required string Name { get; init; }
}