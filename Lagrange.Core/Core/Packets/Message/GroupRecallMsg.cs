using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message;

/// <summary>
/// trpc.msg.msg_svc.MsgService.SsoGroupRecallMsg
/// </summary>
[ProtoContract]
internal class GroupRecallMsg
{
    [ProtoMember(1)] public uint Type { get; set; } // 1
    
    [ProtoMember(2)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public GroupRecallMsgField3 Field3 { get; set; }
    
    [ProtoMember(4)] public GroupRecallMsgField4 Field4 { get; set; }
}

[ProtoContract]
internal class GroupRecallMsgField3
{
    [ProtoMember(1)] public uint Sequence { get; set; }
    
    [ProtoMember(2)] public uint Random { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 0
}

[ProtoContract]
internal class GroupRecallMsgField4
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 0
}