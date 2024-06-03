using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class GroupJoin
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; }

    [ProtoMember(3)] public string TargetUid { get; set; } = string.Empty;
    
    [ProtoMember(4)] public uint Field4 { get; set; }
    
    [ProtoMember(6)] public uint Field6 { get; set; }
    
    [ProtoMember(7)] public string Field7 { get; set; } = string.Empty;
    
    [ProtoMember(8)] public uint Field8 { get; set; }

    [ProtoMember(9)] public byte[] Field9 { get; set; } = Array.Empty<byte>();
}