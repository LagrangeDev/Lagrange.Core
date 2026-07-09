using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupQuitEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class GroupQuitEventResp : ProtocolEvent
{
    public static readonly GroupQuitEventResp Default = new();
}
