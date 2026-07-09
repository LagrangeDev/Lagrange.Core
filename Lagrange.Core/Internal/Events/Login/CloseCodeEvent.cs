using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class CloseCodeEventReq(byte[] k, bool isApproved) : ProtocolEvent
{
    public byte[] K { get; } = k;

    public bool IsApproved { get; } = isApproved;
}

internal class CloseCodeEventResp(byte state, string message) : ProtocolEvent
{
    public byte State { get; } = state;
    
    public string Message { get; } = message;
}