using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class RpsSegment
{
    
}

[SegmentSubscriber(typeof(FaceEntity), "rps")]
public partial class RpsSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is RpsSegment) builder.Face(359, true);
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is FaceEntity { IsLargeFace: true, FaceId: 359 }) return new RpsSegment();

        return null;
    }
}