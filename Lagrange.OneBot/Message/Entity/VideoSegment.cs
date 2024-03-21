using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class VideoSegment(string url)
{
    public VideoSegment() : this("") { }

    [JsonPropertyName("file")] [CQProperty] public string File { get; set; } = url;
    
    [JsonPropertyName("url")] public string Url { get; set; }  = url;
}

[SegmentSubscriber(typeof(VideoEntity), "video")]
public partial class VideoSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is VideoSegment videoSegment and not { File: "" } && CommonResolver.Resolve(videoSegment.File) is { } image)
        {
            builder.Video(image);
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not VideoEntity videoEntity) throw new ArgumentException("Invalid entity type.");

        return new RecordSegment(videoEntity.VideoUrl);
    }
}