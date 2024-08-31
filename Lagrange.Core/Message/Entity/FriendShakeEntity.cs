using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class FriendShakeEntity : IMessageEntity
{
    public ushort FaceId { get; set; }

    public ushort Strength { get; set; }

    public FriendShakeEntity() : this(0, 0) { }

    public FriendShakeEntity(ushort faceId, ushort strength)
    {
        FaceId = faceId;
        Strength = strength;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        byte[] shakePbElem;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, new FriendShakeExtra()
            {
                FaceId = FaceId,
                Strength = Strength
            });
            shakePbElem = ms.ToArray();
        }

        yield return new Elem()
        {
            CommonElem = new()
            {
                ServiceType = 2,
                PbElem = shakePbElem,
                BusinessType = FaceId
            }
        };
    }

    string IMessageEntity.ToPreviewString()
        => $"[FriendShake | Type: {((FriendShakeFaceType)FaceId).TryGetName()}({FaceId}) | Strength: {Strength}]";

    string IMessageEntity.ToPreviewText()
        => $"[{((FriendShakeFaceType)FaceId).TryGetName()}]";

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem is not { ServiceType: 2 })
            return null;

        var shakeExtra = Serializer.Deserialize<FriendShakeExtra>(elem.CommonElem.PbElem.AsSpan());
        return new FriendShakeEntity(shakeExtra.FaceId, shakeExtra.Strength);
    }
}
