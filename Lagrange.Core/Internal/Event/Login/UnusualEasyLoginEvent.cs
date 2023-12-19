namespace Lagrange.Core.Internal.Event.Login;

internal class UnusualEasyLoginEvent : ProtocolEvent
{
    public bool Success { get; set; }
    
    private UnusualEasyLoginEvent() : base(true) { }

    private UnusualEasyLoginEvent(int result) : base(result)
    {
        Success = result == 0;
    }
    
    public static UnusualEasyLoginEvent Create()
    {
        return new UnusualEasyLoginEvent();
    }
    
    public static UnusualEasyLoginEvent Result(int result)
    {
        return new UnusualEasyLoginEvent(result);
    }
}