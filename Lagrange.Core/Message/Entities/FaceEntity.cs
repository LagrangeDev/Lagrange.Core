using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Message.Entities;

public class FaceEntity : IMessageEntity
{
    public int FaceId { get; internal init; }

    public bool IsLarge { get; internal init; }

    Elem[] IMessageEntity.Build() => throw new NotSupportedException();

    IMessageEntity? IMessageEntity.Parse(List<Elem> elements, Elem target)
    {
        if (target.Face is { } face) return new FaceEntity { FaceId = (int)face.Index };

        return target.CommonElem switch
        {
            { ServiceType: 33 } small => new FaceEntity { FaceId = (int)ProtoHelper.Deserialize<QSmallFaceExtra>(small.PbElem.Span).FaceId },
            { ServiceType: 37 } large => new FaceEntity { FaceId = (int)ProtoHelper.Deserialize<QBigFaceExtra>(large.PbElem.Span).FaceId, IsLarge = true },
            _ => null
        };
    }

    string IMessageEntity.ToPreviewString() => "[表情]";
}