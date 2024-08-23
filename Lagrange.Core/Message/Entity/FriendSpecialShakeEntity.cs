using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class FriendSpecialShakeEntity : IMessageEntity
{
    public ushort FaceId { get; set; }

    public uint Count { get; set; }

    public string FaceName { get; set; }

    public FriendSpecialShakeEntity() : this(0, 0, string.Empty) { }

    public FriendSpecialShakeEntity(ushort faceId, uint count, string faceName)
    {
        FaceId = faceId;
        Count = count;
        FaceName = faceName;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        byte[] shakePbElem;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, new FriendSpecialShakeExtra()
            {
                FaceId = FaceId,
                Count = Count,
                FaceName = FaceName
            });
            shakePbElem = ms.ToArray();
        }

        yield return new Elem()
        {
            CommonElem = new()
            {
                ServiceType = 23,
                PbElem = shakePbElem,
                BusinessType = FaceId
            }
        };
    }

    string IMessageEntity.ToPreviewString()
        => $"[SpecialShake]{FaceName}({FaceId})x{Count}";

    string IMessageEntity.ToPreviewText()
        => $"[{FaceName}]x{Count}";

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem is not { ServiceType: 23 })
            return null;

        var specialShakeExtra = Serializer.Deserialize<FriendSpecialShakeExtra>(elem.CommonElem.PbElem.AsSpan());
        return new FriendSpecialShakeEntity(specialShakeExtra.FaceId, specialShakeExtra.Count, specialShakeExtra.FaceName);
    }
}
