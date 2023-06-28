namespace Lagrange.Core.Core.Event.Protocol.Login;

internal class PasswordLoginEvent : ProtocolEvent
{
    public bool Success { get; set; }

    public bool UnusualVerify { get; set; }

    protected PasswordLoginEvent() : base(true) { }

    protected PasswordLoginEvent(bool success, bool unusualVerify) : base(0)
    {
        Success = success;
        UnusualVerify = unusualVerify;
    }

    public static PasswordLoginEvent Create()
    {
        return new PasswordLoginEvent();
    }

    public static PasswordLoginEvent Result(bool success, bool unusualVerify = false)
    {
        return new PasswordLoginEvent(success, unusualVerify);
    }
}