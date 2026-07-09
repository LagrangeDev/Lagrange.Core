using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchClientKeyEventReq : ProtocolEvent;

internal class FetchClientKeyEventResp(string clientKey, uint expiration) : ProtocolEvent
{
    public string ClientKey { get; } = clientKey;

    public uint Expiration { get; } = expiration;
}