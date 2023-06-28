using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class Text
{
    [ProtoMember(1)] public string? Str { get; set; }
    
    [ProtoMember(2)] public string? Link { get; set; }
    
    [ProtoMember(3)] public byte[]? Attr6Buf { get; set; }
    
    [ProtoMember(4)] public byte[]? Attr7Buf { get; set; }
    
    [ProtoMember(11)] public byte[]? Buf { get; set; }
    
    [ProtoMember(12)] public byte[]? PbReserve { get; set; }
}