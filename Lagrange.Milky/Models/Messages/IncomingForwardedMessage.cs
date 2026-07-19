using System.Collections.Generic;
using System.Text.Json.Serialization;
using Lagrange.Milky.Models.Segments;

namespace Lagrange.Milky.Models.Messages;

// public sealed class IncomingForwardedMessage
// {
//     [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
//     [JsonPropertyName("sender_name")] public required string SenderName { get; init; }
//     [JsonPropertyName("avatar_url")] public required string AvatarUrl { get; init; }
//     [JsonPropertyName("time")] public required long Time { get; init; }
//     [JsonPropertyName("segments")] public required IReadOnlyList<IncomingSegmentBase> Segments { get; init; }
// }

public sealed class OutgoingForwardedMessage
{
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("sender_name")] public required string SenderName { get; init; }
    [JsonPropertyName("segments")] public required IReadOnlyList<OutgoingSegmentBase> Segments { get; init; }
}