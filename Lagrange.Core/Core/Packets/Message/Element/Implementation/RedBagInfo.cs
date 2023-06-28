using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class RedBagInfo
{
    [ProtoMember(1)] public uint? RedBagType { get; set; }
}