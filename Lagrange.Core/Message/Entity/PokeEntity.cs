using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class PokeEntity : IMessageEntity
{
    public uint Type { get; }

    public uint Strength { get; }

    internal PokeEntity() { }

    public PokeEntity(uint type, uint strength)
    {
        Type = type;
        Strength = strength;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        byte[] shakePbElem;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, new PokeExtra()
            {
                Type = Type,
                Strength = Strength
            });
            shakePbElem = ms.ToArray();
        }

        return new Elem[]
        {
            new()
            {
                CommonElem = new CommonElem
                {
                    ServiceType = 2,
                    PbElem = shakePbElem,
                    BusinessType = Type
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem is not { ServiceType: 2 })
            return null;

        var poke = Serializer.Deserialize<PokeExtra>(elem.CommonElem.PbElem.AsSpan());
        return new PokeEntity(poke.Type, poke.Strength);
    }

    public string ToPreviewString() => $"[{nameof(PokeEntity)} | Type: {Type} | Strength: {Strength}]";
}