namespace Lagrange.Core.Internal.Event.Action;

internal class SetStatusEvent : ProtocolEvent
{
    public uint Status { get; }
    
    public uint ExtStatus { get; }

    protected SetStatusEvent(uint status, uint extStatus) : base(true)
    {
        Status = status;
        ExtStatus = extStatus;
    }

    protected SetStatusEvent(int resultCode) : base(resultCode) { }

    public static SetStatusEvent Create(uint status, uint extStatus) => new(status, extStatus);

    public static SetStatusEvent Result(int resultCode) => new(resultCode);
}