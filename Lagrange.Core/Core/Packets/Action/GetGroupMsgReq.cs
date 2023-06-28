using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action;

[ProtoContract]
internal class GetGroupMsgReq
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }
    
    [ProtoMember(2)] public ulong BeginSeq { get; set; }
    
    [ProtoMember(3)] public ulong EndSeq { get; set; }
    
    [ProtoMember(4)] public uint Filter { get; set; }
    
    [ProtoMember(5)] public ulong MemberSeq { get; set; }
    
    [ProtoMember(6)] public bool PublicGroup { get; set; }
    
    [ProtoMember(7)] public uint ShieldFlag { get; set; }
    
    [ProtoMember(8)] public uint SaveTrafficFlag { get; set; }
}