using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupRenameEventReq(long groupUin, string targetName) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string TargetName { get; } = targetName;
}

internal class GroupRenameEventResp : ProtocolEvent
{
    public static readonly GroupRenameEventResp Default = new();
}
