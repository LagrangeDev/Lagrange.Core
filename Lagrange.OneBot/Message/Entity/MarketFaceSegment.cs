using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MarketFaceSegment
{
    [JsonPropertyName("face_id")] public string FaceId { get; set; }

    [JsonPropertyName("tab_id")] public int TabId { get; set; }

    [JsonPropertyName("key")] public string Key { get; set; }

    public MarketFaceSegment() : this(string.Empty, default, string.Empty) { }

    public MarketFaceSegment(string faceId, int tabId, string key)
    {
        FaceId = faceId;
        TabId = tabId;
        Key = key;
    }
}

[SegmentSubscriber(typeof(MarketFaceEntity), "marketface")]
public partial class MarketFaceSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is not MarketFaceSegment marketFaceSegment) return;

        builder.Add(new MarketFaceEntity(marketFaceSegment.FaceId, marketFaceSegment.TabId, marketFaceSegment.Key));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not MarketFaceEntity marketFaceEntity) throw new ArgumentException("Invalid entity type.");

        return new MarketFaceSegment(marketFaceEntity.FaceId, marketFaceEntity.TabId, marketFaceEntity.Key);
    }
}