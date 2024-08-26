using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class CustomElem
{
    [ProtoMember(1)] public byte[] Desc { get; set; }

    [ProtoMember(2)] public byte[] Data { get; set; }

    [ProtoMember(3)] public int EnumType { get; set; }

    [ProtoMember(4)] public byte[] Ext { get; set; }

    [ProtoMember(5)] public byte[] Sound { get; set; }
}