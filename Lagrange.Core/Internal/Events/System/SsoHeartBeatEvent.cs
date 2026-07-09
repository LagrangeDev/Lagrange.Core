using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class SsoHeartBeatEventReq : ProtocolEvent;

internal class SsoHeartBeatEventResp(int interval) : ProtocolEvent
{
    public int Interval { get; } = interval;
}