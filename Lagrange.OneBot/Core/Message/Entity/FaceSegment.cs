using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class FaceSegment(int id)
{
    public FaceSegment() : this(0) { }
    
    [JsonPropertyName("id")] [CQProperty] public string Id { get; set; } = id.ToString();
}

[SegmentSubscriber(typeof(FaceEntity), "face")]
public partial class FaceSegment : SegmentBase
{
    private static readonly ushort[] Excluded = [358, 359];
    
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is FaceSegment faceSegment) builder.Face(ushort.Parse(faceSegment.Id));
    }
    
    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not FaceEntity faceEntity) throw new ArgumentException("Invalid entity type.");
        return Excluded.Contains(faceEntity.FaceId) ? null : new FaceSegment(faceEntity.FaceId);
    }
}