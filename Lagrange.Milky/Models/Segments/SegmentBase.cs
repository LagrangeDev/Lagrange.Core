using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Segments;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TextIncomingSegment), "text")]
[JsonDerivedType(typeof(MentionIncomingSegment), "mention")]
[JsonDerivedType(typeof(MentionAllIncomingSegment), "mention_all")]
[JsonDerivedType(typeof(ReplyIncomingSegment), "reply")]
[JsonDerivedType(typeof(ImageIncomingSegment), "image")]
[JsonDerivedType(typeof(RecordIncomingSegment), "record")]
[JsonDerivedType(typeof(VideoIncomingSegment), "video")]
[JsonDerivedType(typeof(FileIncomingSegment), "file")]
[JsonDerivedType(typeof(ForwardIncomingSegment), "forward")]
[JsonDerivedType(typeof(LightAppIncomingSegment), "light_app")]
public abstract class IncomingSegmentBase;
public abstract class IncomingSegmentBase<T> : IncomingSegmentBase
{
    [JsonPropertyName("data")] public required T Data { get; init; }
}