using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class DEB7ReqBody
{
    [ProtoMember(2)] public EB7SignInWriteReq SignInWrite { get; set; }
}

[ProtoPackable]
internal partial class EB7SignInWriteReq
{
    [ProtoMember(1)] public string Uid { get; set; }

    [ProtoMember(2)] public string GroupId { get; set; }

    [ProtoMember(3)] public string ClientVersion { get; set; }
}

[ProtoPackable]
internal partial class DEB7RspBody
{
    [ProtoMember(2)] public EB7SignInWriteRsp? SignInWrite { get; set; }
}

[ProtoPackable]
internal partial class EB7SignInWriteRsp
{
    [ProtoMember(1)] public EB7Ret? Ret { get; set; }

    [ProtoMember(2)] public EB7SignInStatusDoneInfo? DoneInfo { get; set; }

    [ProtoMember(3)] public EB7SignInStatusGroupScore? GroupScore { get; set; }
}

[ProtoPackable]
internal partial class EB7Ret
{
    [ProtoMember(1)] public int Code { get; set; }

    [ProtoMember(2)] public string Msg { get; set; }
}

[ProtoPackable]
internal partial class EB7SignInStatusDoneInfo
{
    [ProtoMember(1)] public string? LeftTitleWord { get; set; }

    [ProtoMember(2)] public string? RightDescWord { get; set; }

    [ProtoMember(3)] public List<string>? BelowPortraitWords { get; set; }

    [ProtoMember(4)] public string? RecordUrl { get; set; }
}

[ProtoPackable]
internal partial class EB7SignInStatusGroupScore
{
    [ProtoMember(1)] public string GroupScoreWord { get; set; }

    [ProtoMember(2)] public string ScoreUrl { get; set; }
}
