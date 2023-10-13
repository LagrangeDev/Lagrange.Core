using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class ImageSegment(string url)
{
    private readonly HttpClient _client = new();

    public ImageSegment() : this("") => _client = new HttpClient();

    [JsonPropertyName("file")] public string Url { get; set; } = url;
}

[SegmentSubscriber(typeof(ImageEntity), "image")]
public partial class ImageSegment : ISegment
{
    public IMessageEntity ToEntity() => new ImageEntity(url);
    
    public void Build(MessageBuilder builder, ISegment segment)
    {
        if (segment is ImageSegment imageSegment)
        {
            if (imageSegment.Url != "") builder.Image(_client.GetAsync(imageSegment.Url).Result.Content.ReadAsByteArrayAsync().Result);
        }
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not ImageEntity imageEntity) throw new ArgumentException("Invalid entity type.");

        return new ImageSegment(imageEntity.ImageUrl);
    }
}