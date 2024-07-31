using Lagrange.Core.Message.Entity;
using Lagrange.Core.Message;
using System.Text.Json.Serialization;
using Lagrange.Core.Utility.Network;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MusicSegment(string? type, string url, string audio, string title, string image, string content, string appid, string sign, string packageName)
{
    public MusicSegment() : this(null, "", "", "", "", "", "", "", "") { }

    [JsonPropertyName("type")][CQProperty] public string? Type { get; set; } = type;

    [JsonPropertyName("id")][CQProperty] public string Id { get; set; } = String.Empty;

    [JsonPropertyName("url")][CQProperty] public string Url { get; set; } = url;

    [JsonPropertyName("audio")][CQProperty] public string Audio { get; set; } = audio;

    [JsonPropertyName("title")][CQProperty] public string Title { get; set; } = title;

    [JsonPropertyName("content")][CQProperty] public string Content { get; set; } = content;

    [JsonPropertyName("image")][CQProperty] public string Image { get; set; } = image;

    [JsonPropertyName("appid")] public string Appid { get; set; } = appid;

    [JsonPropertyName("sign")] public string Sign { get; set; } = sign;

    [JsonPropertyName("package_name")] public string PackageName { get; set; } = packageName;
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
