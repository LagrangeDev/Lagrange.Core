using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupInvitation
{
    [ProtoMember(1)] public int Cmd { get; set; }
    
    [ProtoMember(2)] public InvitationInfo Info { get; set; }
}

[ProtoContract]
internal class InvitationInfo
{
    [ProtoMember(1)] public InvitationInner Inner { get; set; }
}


[ProtoContract]
internal class InvitationInner
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; }
    
    [ProtoMember(4)] public uint Field4 { get; set; }
    
    [ProtoMember(5)] public string TargetUid { get; set; }
    
    [ProtoMember(6)] public string InvitorUid { get; set; }

    [ProtoMember(7)] public uint Field7 { get; set; }
    
    [ProtoMember(9)] public uint Field9 { get; set; }
    
    [ProtoMember(10)] public byte[] Field10 { get; set; }
    
    [ProtoMember(11)] public uint Field11 { get; set; }
    
    [ProtoMember(12)] public string Field12 { get; set; }
}

