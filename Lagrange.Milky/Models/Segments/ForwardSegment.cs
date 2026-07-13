using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class ForwardIncomingSegment : IncomingSegmentBase<ForwardIncomingSegmentData>;
public class ForwardIncomingSegmentData
{
    [JsonPropertyName("forward_id")] public required string ForwardId { get; init; }
    [JsonPropertyName("title")] public required string Title { get; init; }
    [JsonPropertyName("preview")] public required IReadOnlyList<string> Preview { get; init; }
    [JsonPropertyName("summary")] public required string Summary { get; init; }
}