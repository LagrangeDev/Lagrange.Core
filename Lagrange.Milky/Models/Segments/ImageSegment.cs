using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class ImageIncomingSegment : IncomingSegmentBase<ImageIncomingSegmentData>;
public class ImageIncomingSegmentData
{
    [JsonPropertyName("resource_id")] public required string ResourceId { get; init; }
    [JsonPropertyName("temp_url")] public required string TempUrl { get; init; }
    [JsonPropertyName("width")] public required int Width { get; init; }
    [JsonPropertyName("height")] public required int Height { get; init; }
    [JsonPropertyName("summary")] public required string Summary { get; init; }
    [JsonPropertyName("sub_type")] public required string SubType { get; init; }
}