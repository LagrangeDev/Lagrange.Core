using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupAdmin
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Flag { get; set; }
    
    [ProtoMember(3)] public bool IsPromote { get; set; }
    
    [ProtoMember(4)] public GroupAdminBody Body { get; set; }
}

[ProtoContract]
internal class GroupAdminBody
{
    [ProtoMember(1)] public GroupAdminExtra? ExtraDisable { get; set; }
    
    [ProtoMember(2)] public GroupAdminExtra? ExtraEnable { get; set; }
}

[ProtoContract]
internal class GroupAdminExtra
{
    [ProtoMember(1)] public string AdminUid { get; set; }
    
    [ProtoMember(2)] public bool IsPromote { get; set; }
}