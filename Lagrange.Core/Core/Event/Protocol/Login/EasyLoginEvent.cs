namespace Lagrange.Core.Core.Event.Protocol.Login;

internal class EasyLoginEvent : ProtocolEvent
{
    public bool Success { get; set; }
    
    public bool UnusualVerify { get; set; }
    
    protected EasyLoginEvent() : base(true) { }

    protected EasyLoginEvent(bool success, bool unusualVerify) : base(0)
    {
        Success = success;
        UnusualVerify = unusualVerify;
    }
    
    public static EasyLoginEvent Create()
    {
        return new EasyLoginEvent();
    }
    
    public static EasyLoginEvent Result(bool success, bool unusualVerify = false)
    {
        return new EasyLoginEvent(success, unusualVerify);
    }
}