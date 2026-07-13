using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupRemarkEventReq(long groupUin, string targetRemark) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string TargetRemark { get; } = targetRemark;
}

internal class GroupRemarkEventResp : ProtocolEvent
{
    public static readonly GroupRemarkEventResp Default = new();
}
