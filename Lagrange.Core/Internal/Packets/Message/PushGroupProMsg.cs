using Lagrange.Core.Internal.Packets.System;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message;

#pragma warning disable CS8618

[ProtoContract]
internal class PushGroupProMsg
{
    [ProtoMember(1)] public PushGroupProMsgBody Message { get; set; }

    [ProtoMember(3)] public int? Status { get; set; }

    [ProtoMember(4)] public NTSysEvent? NtEvent { get; set; }

    [ProtoMember(5)] public int? PingFLag { get; set; }

    [ProtoMember(9)] public int? GeneralFlag { get; set; }
}

[ProtoContract]
internal class PushGroupProMsgBody
{
    [ProtoMember(1)] public Unknown1 Unknown1 { get; set; }

    [ProtoMember(2)] public PushGroupProMsgContentHead ContentHead { get; set; }

    [ProtoMember(3)] public MessageBody? Body { get; set; }
    
    [ProtoMember(4)] public Unknown4? Unknown4 { get; set; }
}

[ProtoContract]
internal class Unknown1
{
    [ProtoMember(1)] public Unknown1Field1? Field1 { get; set; }
    
    [ProtoMember(2)] public Unknown1Field2? Field2 { get; set; }
}

[ProtoContract]
internal class Unknown1Field1
{
    [ProtoMember(1)] public ulong? GuildId { get; set; }
    
    [ProtoMember(4)] public ulong? SenderId { get; set; }
}

[ProtoContract]
internal class Unknown1Field2
{
    [ProtoMember(4)] public uint? Seq { get; set; }
}


[ProtoContract]
internal class PushGroupProMsgContentHead
{
    [ProtoMember(1)] public uint Type { get; set; }

    [ProtoMember(2)] public uint? SubType { get; set; }

    [ProtoMember(3)] public uint? DivSeq { get; set; }

    [ProtoMember(4)] public long? MsgId { get; set; }

    [ProtoMember(5)] public uint? Sequence { get; set; }

    [ProtoMember(6)] public long? Timestamp { get; set; }

    [ProtoMember(7)] public long? Field7 { get; set; }

    [ProtoMember(8)] public uint? Field8 { get; set; }

    [ProtoMember(9)] public uint? Field9 { get; set; }

    [ProtoMember(11)] public uint? FriendSequence { get; set; }

    [ProtoMember(12)] public ulong? NewId { get; set; }
}

[ProtoContract]
internal class Unknown4
{
    [ProtoMember(1)] public string? SenderNickName { get; set; }
    
    [ProtoMember(2)] public string? GuildName { get; set; }
    
    [ProtoMember(3)] public string? SubGuildName { get; set; }
    
    [ProtoMember(8)] public string? SenderGuildName { get; set; }
    
    [ProtoMember(9)] public uint? Timestamp { get; set; }
}