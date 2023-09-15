using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class CommonElem
{
    [ProtoMember(1)] public int ServiceType { get; set; }
    
    [ProtoMember(2)] public byte[] PbElem { get; set; }
    
    [ProtoMember(3)] public uint BusinessType { get; set; }
}