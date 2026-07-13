using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class TextIncomingSegment : IncomingSegmentBase<TextIncomingSegmentData>;
public class TextIncomingSegmentData
{
    [JsonPropertyName("text")] public required string Text { get; init; }
}