using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class VerifyCodeEventReq(byte[] k) : ProtocolEvent
{
    public byte[] K { get; } = k;
}

internal class VerifyCodeEventResp(byte state, string message, string platform, string location, string? device) : ProtocolEvent
{
    public byte State { get; } = state;
    
    public string Message { get; } = message;
    
    public string Platform { get; } = platform;
    
    public string Location { get; } = location;
    
    public string? Device { get; } = device;
}