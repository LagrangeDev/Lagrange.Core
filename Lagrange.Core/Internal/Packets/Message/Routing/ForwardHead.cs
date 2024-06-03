using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Routing;

[ProtoContract]
internal class ForwardHead
{
    [ProtoMember(1)] public uint? Field1 { get; set; } // 0
    
    [ProtoMember(2)] public uint? Field2 { get; set; } // 0
    
    [ProtoMember(3)] public uint? Field3 { get; set; } // for friend: 2, for group: null
    
    [ProtoMember(4)] public string? UnknownBase64 { get; set; }
    
    [ProtoMember(5)] public string? Avatar { get; set; }
}