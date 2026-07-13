using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class FileIncomingSegment : IncomingSegmentBase<FileIncomingSegmentData>;
public class FileIncomingSegmentData
{
    [JsonPropertyName("file_id")] public required string FileId { get; init; }
    [JsonPropertyName("file_name")] public required string FileName { get; init; }
    [JsonPropertyName("file_size")] public required long FileSize { get; init; }
    [JsonPropertyName("file_hash")] public required string? FileHash { get; init; }
}