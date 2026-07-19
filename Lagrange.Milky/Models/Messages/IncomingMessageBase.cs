using System.Collections.Generic;
using System.Text.Json.Serialization;
using Lagrange.Milky.Models.Segments;

namespace Lagrange.Milky.Models.Messages;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "message_scene")]
[JsonDerivedType(typeof(FriendIncomingMessage), "friend")]
[JsonDerivedType(typeof(GroupIncomingMessage), "group")]
[JsonDerivedType(typeof(TempIncomingMessage), "temp")]
public abstract class IncomingMessageBase
{
    [JsonPropertyName("peer_id")] public required long PeerId { get; init; }
    [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
    [JsonPropertyName("sender_id")] public required long SenderId { get; init; }
    [JsonPropertyName("time")] public required long Time { get; init; }
    [JsonPropertyName("segments")] public required IReadOnlyList<IncomingSegmentBase> Segments { get; init; }
}
