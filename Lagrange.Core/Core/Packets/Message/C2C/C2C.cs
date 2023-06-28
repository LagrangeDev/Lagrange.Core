using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.C2C;

[ProtoContract]
internal class C2C
{
    [ProtoMember(1)] public uint? Uin { get; set; }
    
    [ProtoMember(2)] public string? Uid { get; set; }
    
    [ProtoMember(3)] public uint? Field3 { get; set; }
    
    [ProtoMember(4)] public uint? Sig { get; set; }
    
    [ProtoMember(5)] public uint? ReceiverUin { get; set; }
    
    [ProtoMember(6)] public string? ReceiverUid { get; set; }
}