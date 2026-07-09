using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class KeyExchangeEventReq : ProtocolEvent;

internal class KeyExchangeEventResp(byte[] sessionTicket, byte[] sessionKey) : ProtocolEvent
{
    public byte[] SessionTicket { get; } = sessionTicket;

    public byte[] SessionKey { get; } = sessionKey;
}