namespace Lagrange.Core.Internal.Event;

internal class ProtocolEvent 
{
    public bool WaitResponse { get; }
    
    public int ResultCode { get; private set; }

    protected ProtocolEvent(bool waitResponse) => WaitResponse = waitResponse;
    
    protected ProtocolEvent(int resultCode) => ResultCode = resultCode;
}