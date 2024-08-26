using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class DiceSegment
{

}

[SegmentSubscriber(typeof(FaceEntity), "dice")]
public partial class DiceSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is DiceSegment) builder.Face(358, true);
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is FaceEntity { IsLargeFace: true, FaceId: 358 }) return new DiceSegment();

        return null;
    }
}