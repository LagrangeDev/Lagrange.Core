using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupKickMemberEventReq(long groupUin, string targetUid, bool rejectAddRequest, string reason) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string TargetUid { get; } = targetUid;

    public bool RejectAddRequest { get; } = rejectAddRequest;

    public string Reason { get; } = reason;
}

internal class GroupKickMemberEventResp(int resultCode, string? errorMsg) : ProtocolEvent
{
    public int ResultCode { get; } = resultCode;

    public string? ErrorMsg { get; } = errorMsg;
}
