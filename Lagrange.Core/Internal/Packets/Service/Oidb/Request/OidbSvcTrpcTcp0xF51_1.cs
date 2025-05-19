using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xF51_1
{
    [ProtoMember(1)]
    public OidbSvcTrpcTcp0xF51_1C2CMsgInfo? C2CMsgInfo { get; set; }

    [ProtoMember(2)]
    public OidbSvcTrpcTcp0xF51_1GroupMsgInfo? GroupMsgInfo { get; set; }

    [ProtoMember(3)]
    public OidbSvcTrpcTcp0xF51_1CommGrayTipsInfo CommGrayTipsInfo { get; set; }
}


[ProtoContract]
internal class OidbSvcTrpcTcp0xF51_1C2CMsgInfo
{
    [ProtoMember(1)]
    public ulong AioUin { get; set; }

    [ProtoMember(2)]
    public ulong MsgType { get; set; }

    [ProtoMember(3)]
    public ulong MsgSeq { get; set; }

    [ProtoMember(4)]
    public ulong MsgTime { get; set; }

    [ProtoMember(5)]
    public ulong MsgUid { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xF51_1GroupMsgInfo
{
    [ProtoMember(1)]
    public ulong GroupCode { get; set; }

    [ProtoMember(2)]
    public ulong MsgType { get; set; }

    [ProtoMember(3)]
    public ulong MsgSeq { get; set; }

    [ProtoMember(4)]
    public ulong MsgTime { get; set; }

    [ProtoMember(5)]
    public ulong MsgUid { get; set; }

    [ProtoMember(6)]
    public ulong MsgId { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xF51_1CommGrayTipsInfo
{
    [ProtoMember(1)]
    public ulong BusiId { get; set; }

    [ProtoMember(2)]
    public ulong TipsSeqId { get; set; }
}