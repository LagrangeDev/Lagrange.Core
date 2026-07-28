using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupMuteMemberEventReq(long groupUin, string targetUid, uint duration) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string TargetUid { get; } = targetUid;

    public uint Duration { get; } = duration;
}

internal class GroupMuteMemberEventResp : ProtocolEvent
{
    public static readonly GroupMuteMemberEventResp Default = new();
}
