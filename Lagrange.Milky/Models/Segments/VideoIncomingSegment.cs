using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class VideoIncomingSegment : IncomingSegmentBase<VideoIncomingSegmentData>;
public class VideoIncomingSegmentData
{
    [JsonPropertyName("resource_id")] public required string ResourceId { get; init; }
    [JsonPropertyName("temp_url")] public required string TempUrl { get; init; }
    [JsonPropertyName("width")] public required int Width { get; init; }
    [JsonPropertyName("height")] public required int Height { get; init; }
    [JsonPropertyName("duration")] public required int Duration { get; init; }
}