using Lagrange.Core.Message.Entity;
using Lagrange.Core.Message;
using System.Text.Json.Serialization;
using Lagrange.Core.Utility.Network;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MusicSegment(string type, string url, string audio, string title, string image, string singer)
{
    public MusicSegment() : this("","","","","","") { }

    [JsonPropertyName("type")][CQProperty] public string Type { get; set; } = type;

    [JsonPropertyName("url")][CQProperty] public string Url { get; set; } = url;

    [JsonPropertyName("audio")][CQProperty] public string Audio { get; set; } = audio;

    [JsonPropertyName("title")][CQProperty] public string Title { get; set; } = title;

    [JsonPropertyName("image")][CQProperty] public string Image { get; set; } = image;

    [JsonPropertyName("singer")][CQProperty] public string Singer { get; set; } = singer;
}

[SegmentSubscriber(typeof(ImageEntity), "music")]
public partial class MusicSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is MusicSegment musicSegment)
        {
            var content = MusicSigner.Sign(musicSegment);
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("music SignerServer response Errors!");
            builder.LightApp(content);
        }
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        return null;
    }
}
