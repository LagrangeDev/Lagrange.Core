using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

/// <summary>
/// trpc.group.long_msg_interface.MsgService.SsoRecvLongMsg Request
/// </summary>
[ProtoContract]
internal class RecvLongMsgReq
{
    [ProtoMember(1)] public RecvLongMsgInfo? Info { get; set; }
    
    [ProtoMember(15)] public LongMsgSettings? Settings { get; set; }
}

[ProtoContract]
internal class RecvLongMsgInfo
{
    [ProtoMember(1)] public LongMsgUid? Uid { get; set; }
    
    [ProtoMember(2)] public string? ResId { get; set; }
    
    [ProtoMember(3)] public bool Acquire { get; set; } // true
}

[ProtoContract]
internal class LongMsgUid
{
    [ProtoMember(2)] public string? Uid { get; set; }
}

[ProtoContract]
internal class LongMsgSettings
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 4
    
    [ProtoMember(2)] public uint Field2 { get; set; } // 1
    
    [ProtoMember(3)] public uint Field3 { get; set; } // 7
    
    [ProtoMember(4)] public uint? Field4 { get; set; } // 0
}