using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupMuteGlobalEventReq(long groupUin, bool isMute) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public bool IsMute { get; } = isMute;
}

internal class GroupMuteGlobalEventResp : ProtocolEvent
{
    public static readonly GroupMuteGlobalEventResp Default = new();
}
