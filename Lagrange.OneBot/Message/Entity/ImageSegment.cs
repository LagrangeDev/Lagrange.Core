using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class ImageSegment(string url)
{
    public ImageSegment() : this("") { }
    
    public ImageSegment(string url, int? picSubType = null) : this()
    {
        this.File = url;
        this.Url = url;
        this.PicSubType = picSubType ?? -1;
    }

    [JsonPropertyName("file")] [CQProperty] public string File { get; set; } = url;
    
    [JsonPropertyName("url")] public string Url { get; set; }  = url;
    
    [JsonPropertyName("subType")] public int? PicSubType { get; set; } = -1;
}

[SegmentSubscriber(typeof(ImageEntity), "image")]
public partial class ImageSegment : SegmentBase
{
    // when will use?
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is ImageSegment imageSegment and not { File: "" } && CommonResolver.ResolveStream(imageSegment.File) is { } stream)
        {
            builder.Add(new ImageEntity
            {
                FilePath = "",
                ImageStream = stream,
                PicSubType = imageSegment.PicSubType,
            });
        }
    }

    // used
    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not ImageEntity imageEntity) throw new ArgumentException("Invalid entity type.");

        return new ImageSegment(imageEntity.ImageUrl, imageEntity.PicSubType);
    }
}