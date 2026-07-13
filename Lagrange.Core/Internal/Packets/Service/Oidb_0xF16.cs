using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class DF16ReqBody
{
    [ProtoMember(1)] public F16GroupRemarkInfoReq RemarkInfo { get; set; }
}

[ProtoPackable]
internal partial class F16GroupRemarkInfoReq
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong? GroupUin { get; set; }

    [ProtoMember(3)] public byte[] RemarkName { get; set; }
}

[ProtoPackable]
internal partial class DF16RspBody
{
    [ProtoMember(1)] public F16GroupRemarkInfoRsp? RemarkInfo { get; set; }
}

[ProtoPackable]
internal partial class F16GroupRemarkInfoRsp
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong GroupUin { get; set; }

    [ProtoMember(3)] public int Result { get; set; }
}
