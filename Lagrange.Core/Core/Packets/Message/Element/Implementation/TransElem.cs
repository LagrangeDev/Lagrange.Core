using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class TransElem
{
    [ProtoMember(1)] public int ElemType { get; set; }
    
    [ProtoMember(2)] public byte[] ElemValue { get; set; }
}