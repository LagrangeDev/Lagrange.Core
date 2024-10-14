using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class FaceSegment(int id, string? type)
{
    public FaceSegment() : this(0, null) { }

    [JsonPropertyName("id")][CQProperty] public string Id { get; set; } = id.ToString();
    [JsonPropertyName("type")][CQProperty] public string? Type { get; set; } = type;
}

[SegmentSubscriber(typeof(FaceEntity), "face")]
public partial class FaceSegment : SegmentBase
{
    private static readonly ushort[] Excluded = [358, 359];

    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is FaceSegment faceSegment) builder.Face(ushort.Parse(faceSegment.Id), faceSegment.Type == "sticker" || false);
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not FaceEntity faceEntity) throw new ArgumentException("Invalid entity type.");
        return Excluded.Contains(faceEntity.FaceId) ? null : new FaceSegment(faceEntity.FaceId, faceEntity.IsLargeFace ? "sticker" : null);
    }
}