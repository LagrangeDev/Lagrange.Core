using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class VideoSegment(string url)
{
    public VideoSegment() : this("") { }

    [JsonPropertyName("file")] public string Url { get; set; } = url;
}

[SegmentSubscriber(typeof(VideoEntity), "video")]
public partial class VideoSegment : ISegment
{
    public IMessageEntity ToEntity() => new VideoEntity(Url);
    
    public void Build(MessageBuilder builder, ISegment segment)
    {
        if (segment is VideoSegment videoSegment and not { Url: "" } && CommonResolver.Resolve(videoSegment.Url) is { } image)
        {
            // TODO: Add Video
        }
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not VideoEntity videoEntity) throw new ArgumentException("Invalid entity type.");

        return new RecordSegment(videoEntity.VideoUrl);
    }
}