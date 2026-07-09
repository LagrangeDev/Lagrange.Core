using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupSetSpecialTitleEventReq(long groupUin, string targetUid, string title) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string TargetUid { get; } = targetUid;

    public string Title { get; } = title;
}

internal class GroupSetSpecialTitleEventResp : ProtocolEvent
{
    public static readonly GroupSetSpecialTitleEventResp Default = new();
}
