using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class DF51ReqBody
{
    [ProtoMember(1)] public F51C2CMsgInfo? C2CMsgInfo { get; set; }

    [ProtoMember(2)] public F51GroupMsgInfo? GroupMsgInfo { get; set; }

    [ProtoMember(3)] public F51CommGrayTipsInfo CommGrayTipsInfo { get; set; }
}

[ProtoPackable]
internal partial class F51C2CMsgInfo
{
    [ProtoMember(1)] public ulong AioUin { get; set; }

    [ProtoMember(2)] public ulong MsgType { get; set; }

    [ProtoMember(3)] public ulong MsgSeq { get; set; }

    [ProtoMember(4)] public ulong MsgTime { get; set; }

    [ProtoMember(5)] public ulong MsgUid { get; set; }
}

[ProtoPackable]
internal partial class F51GroupMsgInfo
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong MsgType { get; set; }

    [ProtoMember(3)] public ulong MsgSeq { get; set; }

    [ProtoMember(4)] public ulong MsgTime { get; set; }

    [ProtoMember(5)] public ulong MsgUid { get; set; }

    [ProtoMember(6)] public ulong MsgId { get; set; }
}

[ProtoPackable]
internal partial class F51CommGrayTipsInfo
{
    [ProtoMember(1)] public ulong BusiId { get; set; }

    [ProtoMember(2)] public ulong TipsSeqId { get; set; }
}

[ProtoPackable]
internal partial class DF51RspBody;
