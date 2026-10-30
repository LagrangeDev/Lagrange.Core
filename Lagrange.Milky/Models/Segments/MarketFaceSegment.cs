using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class MarketFaceIncomingSegment : IncomingSegmentBase<MarketFaceIncomingSegmentData>;
public class MarketFaceIncomingSegmentData
{
    [JsonPropertyName("emoji_package_id")] public required int EmojiPackageId { get; init; }
    [JsonPropertyName("emoji_id")] public required string EmojiId { get; init; }
    [JsonPropertyName("key")] public required string Key { get; init; }
    [JsonPropertyName("summary")] public required string Summary { get; init; }
    [JsonPropertyName("url")] public required string Url { get; init; }
}