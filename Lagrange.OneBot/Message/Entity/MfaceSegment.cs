using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Network;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MfaceSegment(string? url, string emojiId, int emojiPackageId, string key, string? summary)
{
    [JsonPropertyName("url")] public string? Url { get; set; } = url;

    [JsonPropertyName("emoji_package_id")] public int EmojiPackageId { get; set; } = emojiPackageId;

    [JsonPropertyName("emoji_id")] public string EmojiId { get; set; } = emojiId;

    [JsonPropertyName("key")] public string Key { get; set; } = key;

    [JsonPropertyName("summary")] public string? Summary { get; set; } = summary;

    public MfaceSegment() : this(null, string.Empty, default, string.Empty, null) { }
}

[SegmentSubscriber(typeof(MarketfaceEntity), "mface")]
public partial class MfaceSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is not MfaceSegment mfs) return;

        if (mfs.Summary == null)
        {
            JsonElement tabJson = JsonDocument.Parse(
                Http.GetAsync(
                    $"https://i.gtimg.cn/club/item/parcel/{mfs.EmojiPackageId % 10}/{mfs.EmojiPackageId}.json"
                ).Result
            ).RootElement;

            foreach (JsonElement imgJson in tabJson.GetProperty("imgs").EnumerateArray())
            {
                if (imgJson.GetProperty("id").GetString() == mfs.EmojiId)
                {
                    mfs.Summary = $"[{imgJson.GetProperty("name").GetString()}]" ?? "[\u5546\u57ce\u8868\u60c5]";
                    break;
                }
            }

            mfs.Summary ??= "[\u5546\u57ce\u8868\u60c5]";
        }

        builder.Add(new MarketfaceEntity(mfs.EmojiId, mfs.EmojiPackageId, mfs.Key, mfs.Summary));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not MarketfaceEntity mfe) throw new ArgumentException("Invalid entity type.");

        return new MfaceSegment($"https://gxh.vip.qq.com/club/item/parcel/item/{mfe.EmojiId[..2]}/{mfe.EmojiId}/raw300.gif", mfe.EmojiId, mfe.EmojiPackageId, mfe.Key, mfe.Summary);
    }
}