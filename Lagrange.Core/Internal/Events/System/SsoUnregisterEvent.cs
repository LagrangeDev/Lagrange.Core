using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class SsoUnregisterEventReq : ProtocolEvent;

internal class SsoUnregisterEventResp(string message) : ProtocolEvent
{
    public string Message { get; } = message;
}