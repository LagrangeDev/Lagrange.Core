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
        if (segment is ImageSegment imageSegment and not { Url: "" })
        {
            if (imageSegment.Url.StartsWith("http"))
            {
                builder.Image(_client.GetAsync(imageSegment.Url).Result.Content.ReadAsByteArrayAsync().Result);
            }
                
            if (imageSegment.Url.StartsWith("file"))
            {
                string path = imageSegment.Url.Replace("file:///", "");
                builder.Image(File.ReadAllBytes(path));
            }
                
            if (imageSegment.Url.StartsWith("base64"))
            {
                string base64 = imageSegment.Url.Replace("base64://", "");
                builder.Image(Convert.FromBase64String(base64));
            }
        }
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not ImageEntity imageEntity) throw new ArgumentException("Invalid entity type.");

        return new ImageSegment(imageEntity.ImageUrl);
    }
}