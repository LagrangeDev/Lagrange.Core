using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupTransferEventReq(long groupUin, long targetUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public long TargetUin { get; } = targetUin;
}

internal class GroupTransferEventResp : ProtocolEvent
{
    public static readonly GroupTransferEventResp Default = new();
}
