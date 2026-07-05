using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class KickNTReq
{
    [ProtoMember(1)] public long Uin { get; set; }

    [ProtoMember(2)] public bool IsSameDevice { get; set; }

    [ProtoMember(3)] public string TipsInfo { get; set; }

    [ProtoMember(4)] public string TipsTitle { get; set; }
}

[ProtoPackable]
internal partial class SetStatusReq
{
    [ProtoMember(1)] public int Status { get; set; }

    [ProtoMember(2)] public int ExtStatus { get; set; }

    [ProtoMember(3)] public int BatteryStatus { get; set; }

    [ProtoMember(4)] public SetStatusCustomExt? CustomExt { get; set; }
}

[ProtoPackable]
internal partial class SetStatusCustomExt
{
    [ProtoMember(1)] public ulong FaceId { get; set; }

    [ProtoMember(2)] public string Wording { get; set; }

    [ProtoMember(3)] public ulong FaceType { get; set; }
}

[ProtoPackable]
internal partial class SetStatusResp
{
    [ProtoMember(1)] public int ErrCode { get; set; }

    [ProtoMember(2)] public string ErrMsg { get; set; }
}
