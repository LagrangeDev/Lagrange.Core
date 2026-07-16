using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

public class ReplyIncomingSegment : IncomingSegmentBase<ReplyIncomingSegmentData>;
public class ReplyIncomingSegmentData
{
    [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
    [JsonPropertyName("sender_id")] public required long SenderId { get; init; }
    [JsonPropertyName("sender_name")] public required string? SenderName { get; init; }
    [JsonPropertyName("time")] public required long Time { get; init; }
    [JsonPropertyName("segments")] public required IReadOnlyList<IncomingSegmentBase> Segments { get; init; }
}