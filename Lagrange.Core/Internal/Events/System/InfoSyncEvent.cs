using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class InfoSyncEventReq : ProtocolEvent;

internal class InfoSyncEventResp(string message) : ProtocolEvent
{
    public string Message { get; } = message;
}