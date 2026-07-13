using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class RecordIncomingSegment : IncomingSegmentBase<RecordIncomingSegmentData>;
public class RecordIncomingSegmentData
{
    [JsonPropertyName("resource_id")] public required string ResourceId { get; init; }
    [JsonPropertyName("temp_url")] public required string TempUrl { get; init; }
    [JsonPropertyName("duration")] public required int Duration { get; init; }
}