using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component;

[ProtoContract]
internal class GroupInfo
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }
    
    [ProtoMember(2)] public int GroupType { get; set; }
    
    [ProtoMember(3)] public long GroupInfoSeq { get; set; }
    
    [ProtoMember(4)] public string GroupCard { get; set; }
    
    [ProtoMember(5)] public int GroupLevel { get; set; }
    
    [ProtoMember(6)] public int GroupCardType { get; set; }
    
    [ProtoMember(7)] public byte[] GroupName { get; set; }
}