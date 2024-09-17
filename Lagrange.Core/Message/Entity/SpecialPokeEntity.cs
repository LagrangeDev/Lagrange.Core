using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class SpecialPokeEntity : IMessageEntity
{
    public uint FaceId { get; set; }

    public uint Count { get; set; }

    public string FaceName { get; set; }

    public SpecialPokeEntity() : this(0, 0, string.Empty) { }

    public SpecialPokeEntity(uint faceId, uint count, string faceName)
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
            Serializer.Serialize(ms, new SpecialPokeExtra()
            {
                Type = FaceId,
                Count = Count,
                FaceName = FaceName
            });
            shakePbElem = ms.ToArray();
        }

        return new Elem[]
        {
            new()
            {
                CommonElem = new()
                {
                    ServiceType = 23,
                    PbElem = shakePbElem,
                    BusinessType = FaceId
                }
            }
        };
    }

    string IMessageEntity.ToPreviewString()
        => $"[SpecialShake | Name: {FaceName}({FaceId}) | Count: {Count}]";

    string IMessageEntity.ToPreviewText()
        => $"[{FaceName}]x{Count}";

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem is not { ServiceType: 23 })
            return null;

        var specialPokeExtra = Serializer.Deserialize<SpecialPokeExtra>(elem.CommonElem.PbElem.AsSpan());
        return new SpecialPokeEntity(specialPokeExtra.Type, specialPokeExtra.Count, specialPokeExtra.FaceName);
    }
}
