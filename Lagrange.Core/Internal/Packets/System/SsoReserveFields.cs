using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class SsoReserveFields
{
    [ProtoMember(4)] public byte[]? ClientIpcookie { get; set; }

    [ProtoMember(9)] public uint? Flag { get; set; }

    [ProtoMember(10)] public uint? EnvId { get; set; }

    [ProtoMember(11)] public uint? LocaleId { get; set; }

    [ProtoMember(12)] public string? Qimei { get; set; }

    [ProtoMember(13)] public byte[]? Env { get; set; }

    [ProtoMember(14)] public uint? NewconnFlag { get; set; }

    [ProtoMember(15)] public string? TraceParent { get; set; }

    [ProtoMember(16)] public string? Uid { get; set; }

    [ProtoMember(18)] public uint? Imsi { get; set; }

    [ProtoMember(19)] public uint? NetworkType { get; set; }

    [ProtoMember(20)] public uint? IpStackType { get; set; }

    [ProtoMember(21)] public uint? MsgType { get; set; }

    [ProtoMember(22)] public string? TrpcRsp { get; set; }

    [ProtoMember(23)] public Dictionary<string, string>? TransInfo { get; set; }

    [ProtoMember(24)] public SsoSecureInfo? SecInfo { get; set; }

    [ProtoMember(25)] public uint? SecSigFlag { get; set; }

    [ProtoMember(26)] public uint? NtCoreVersion { get; set; }

    [ProtoMember(27)] public uint? SsoRouteCost { get; set; }

    [ProtoMember(28)] public uint? SsoIpOrigin { get; set; }

    [ProtoMember(30)] public byte[]? PresureToken { get; set; }
}