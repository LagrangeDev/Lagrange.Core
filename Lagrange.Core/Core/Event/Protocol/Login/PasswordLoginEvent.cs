namespace Lagrange.Core.Core.Event.Protocol.Login;

internal class PasswordLoginEvent : ProtocolEvent
{
    public bool Success { get; set; }

    public bool UnusualVerify { get; set; }
    
    public string? Tag { get; set; }
    
    public string? Message { get; set; }

    private PasswordLoginEvent() : base(true) { }

    private PasswordLoginEvent(bool success, bool unusualVerify, string? tag = null, string? message = null) : base(0)
    {
        Success = success;
        UnusualVerify = unusualVerify;
        Tag = tag;
        Message = message;
    }

    public static PasswordLoginEvent Create()
    {
        return new PasswordLoginEvent();
    }

    public static PasswordLoginEvent Result(bool success, bool unusualVerify = false, string? tag = null, string? message = null)
    {
        return new PasswordLoginEvent(success, unusualVerify, tag, message);
    }
}