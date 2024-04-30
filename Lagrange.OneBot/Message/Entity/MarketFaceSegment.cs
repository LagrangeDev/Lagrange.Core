using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Network;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MarketFaceSegment(string faceId, int tabId, string key, string? summary)
{
    [JsonPropertyName("face_id")] public string FaceId { get; set; } = faceId;

    [JsonPropertyName("tab_id")] public int TabId { get; set; } = tabId;

    [JsonPropertyName("key")] public string Key { get; set; } = key;

    [JsonPropertyName("summary")] public string? Summary { get; set; } = summary;

    public MarketFaceSegment() : this(string.Empty, default, string.Empty, null) { }
}

[SegmentSubscriber(typeof(MarketFaceEntity), "marketface")]
public partial class MarketFaceSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is not MarketFaceSegment mfs) return;

        if (mfs.Summary == null)
        {
            JsonElement tabJson = JsonDocument.Parse(
                Http.GetAsync(
                    $"https://i.gtimg.cn/club/item/parcel/{mfs.TabId % 10}/{mfs.TabId}.json"
                ).Result
            ).RootElement;

            foreach (JsonElement imgJson in tabJson.GetProperty("imgs").EnumerateArray())
            {
                if (imgJson.GetProperty("id").GetString() == mfs.FaceId)
                {
                    mfs.Summary = $"[{imgJson.GetProperty("name").GetString()}]" ?? "[\u5546\u57ce\u8868\u60c5]";
                    break;
                }
            }

            mfs.Summary ??= "[\u5546\u57ce\u8868\u60c5]";
        }

        builder.Add(new MarketFaceEntity(mfs.FaceId, mfs.TabId, mfs.Key, mfs.Summary));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not MarketFaceEntity mfe) throw new ArgumentException("Invalid entity type.");

        return new MarketFaceSegment(mfe.FaceId, mfe.TabId, mfe.Key, mfe.Summary);
    }
}