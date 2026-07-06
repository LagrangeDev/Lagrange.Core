using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D8A0ReqBody
{
    [ProtoMember(1)] public ulong OptUint64GroupCode { get; set; }

    [ProtoMember(2)] public List<KickMemberInfo> RptMsgKickList { get; set; }

    [ProtoMember(3)] public List<ulong> RptKickList { get; set; }

    [ProtoMember(4)] public uint Uint32KickFlag { get; set; }

    [ProtoMember(5)] public byte[] BytesKickMsg { get; set; }
}

[ProtoPackable]
internal partial class D8A0RspBody
{
    [ProtoMember(1)] public ulong OptUint64GroupCode { get; set; }

    [ProtoMember(2)] public List<KickResult> RptMsgKickResult { get; set; }
}

[ProtoPackable]
internal partial class KickMemberInfo
{
    [ProtoMember(1)] public uint OptUint32Operate { get; set; }

    [ProtoMember(2)] public ulong OptUint64MemberUin { get; set; }

    [ProtoMember(3)] public uint OptUint32Flag { get; set; }

    [ProtoMember(4)] public byte[] OptBytesMsg { get; set; }
}

[ProtoPackable]
internal partial class KickResult
{
    [ProtoMember(1)] public uint OptUint32Result { get; set; }

    [ProtoMember(2)] public ulong OptUint64MemberUin { get; set; }
}

[ProtoPackable]
internal partial class D8A0_1ReqBody
{
    [ProtoMember(1)] public ulong GroupUin { get; set; }

    [ProtoMember(3)] public string TargetUid { get; set; }

    [ProtoMember(4)] public bool RejectAddRequest { get; set; }

    [ProtoMember(5)] public string Reason { get; set; }
}

[ProtoPackable]
internal partial class D8A0_1RspBody
{
    [ProtoMember(1)] public ulong GroupUin { get; set; }

    [ProtoMember(2)] public string? ErrorMsg { get; set; }
}
