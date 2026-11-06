using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class FaceIncomingSegment : IncomingSegmentBase<FaceIncomingSegmentData>;
public class FaceIncomingSegmentData
{
    [JsonPropertyName("face_id")] public required string FaceId { get; init; }
    [JsonPropertyName("is_large")] public required bool IsLarge { get; init; }
}