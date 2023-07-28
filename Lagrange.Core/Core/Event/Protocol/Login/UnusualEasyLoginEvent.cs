namespace Lagrange.Core.Core.Event.Protocol.Login;

internal class UnusualEasyLoginEvent : ProtocolEvent
{
    public bool Success { get; set; }
    
    protected UnusualEasyLoginEvent() : base(true) { }

    protected UnusualEasyLoginEvent(bool success) : base(0)
    {
        Success = success;
    }
    
    public static UnusualEasyLoginEvent Create()
    {
        return new UnusualEasyLoginEvent();
    }
    
    public static UnusualEasyLoginEvent Result(bool success)
    {
        return new UnusualEasyLoginEvent(success);
    }
}