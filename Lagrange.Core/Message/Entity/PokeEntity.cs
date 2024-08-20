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
        var stream = new MemoryStream();
        Serializer.Serialize(stream, new PokeExtra
        {
            Type = Type,
            Strength = Strength,
            Field8 = 0
        });

        return new Elem[]
        {
            new()
            {
                CommonElem = new CommonElem
                {
                    ServiceType = 2,
                    BusinessType = 1,
                    PbElem = stream.ToArray()
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem is { CommonElem: { ServiceType: 2, BusinessType: 1 } common })
        {
            var poke = Serializer.Deserialize<PokeExtra>(common.PbElem.AsSpan());
            return new PokeEntity(poke.Type, poke.Strength);
        }

        return null;
    }

    public string ToPreviewString() => $"[{nameof(PokeEntity)} | Type: {Type} | Strength: {Strength}]";
}