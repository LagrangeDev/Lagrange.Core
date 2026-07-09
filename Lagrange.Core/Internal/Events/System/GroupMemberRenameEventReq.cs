using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupMemberRenameEventReq(long groupUin, string targetUid, string name) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string TargetUid { get; } = targetUid;

    public string Name { get; } = name;
}

internal class GroupMemberRenameEventResp : ProtocolEvent
{
    public static readonly GroupMemberRenameEventResp Default = new();
}