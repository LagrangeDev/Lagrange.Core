using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class ImageSegment(string url, string summary = "[图片]")
{
    public ImageSegment() : this("", "") { }

    [JsonPropertyName("file")] [CQProperty] public string File { get; set; } = url;
    
    [JsonPropertyName("url")] public string Url { get; set; }  = url;

    [JsonPropertyName("summary")] public string Summary { get; set; } = summary;
}

[SegmentSubscriber(typeof(ImageEntity), "image")]
public partial class ImageSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is ImageSegment imageSegment and not { File: "" } && CommonResolver.ResolveStream(imageSegment.File) is { } stream)
        {
            builder.Add(new ImageEntity
            {
                FilePath = "",
                ImageStream = new Lazy<Stream>(stream),
                Summary = imageSegment.Summary
            });
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not ImageEntity imageEntity) throw new ArgumentException("Invalid entity type.");

        return new ImageSegment(imageEntity.ImageUrl, imageEntity.ToPreviewText());
    }
}
