using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x543)]
[ProtoContract]
internal class Tlv543 : TlvBody
{
    [ProtoMember(9)] public Tlv543Layer1 Layer1 { get; set; }
}

[ProtoContract]
internal class Tlv543Layer1
{
    [ProtoMember(11)] public Tlv543Layer2 Layer2 { get; set; }
}

[ProtoContract]
internal class Tlv543Layer2
{
    [ProtoMember(1)] public string Uid { get; set; }
}