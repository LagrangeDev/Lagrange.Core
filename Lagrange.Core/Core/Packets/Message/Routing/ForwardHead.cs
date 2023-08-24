using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Routing;

[ProtoContract]
internal class ForwardHead
{
    [ProtoMember(3)] public uint? Field3 { get; set; } // for friend: 2, for group: null
    
    [ProtoMember(4)] public string? UnknownBase64 { get; set; }
    
    [ProtoMember(5)] public string? Avatar { get; set; }
}