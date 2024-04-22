using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MarketFaceSegment
{
    [JsonPropertyName("face_id")] public string FaceId { get; set; } = string.Empty;

    [JsonPropertyName("tab_id")] public int TabId { get; set; }

    public MarketFaceSegment() { }

    public MarketFaceSegment(string faceId, int tabId)
    {
        FaceId = faceId;
        TabId = tabId;
    }
}

[SegmentSubscriber(typeof(MarketFaceEntity), "marketface")]
public partial class MarketFaceSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is not MarketFaceSegment marketFaceSegment) return;

        builder.Add(new MarketFaceEntity(marketFaceSegment.FaceId, marketFaceSegment.TabId));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not MarketFaceEntity marketFaceEntity) throw new ArgumentException("Invalid entity type.");

        return new MarketFaceSegment(marketFaceEntity.FaceId, marketFaceEntity.TabId);
    }
}