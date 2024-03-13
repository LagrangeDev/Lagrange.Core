namespace Lagrange.Core.Internal.Event.System;

internal class LoginNotifyEvent : ProtocolEvent
{
    public bool IsLogin { get; }
    
    public uint AppId { get; }
    
    public string Tag { get; }
    
    public string Message { get; }

    private LoginNotifyEvent(bool isLogin, uint appId, string tag, string message) : base(0)
    {
        IsLogin = isLogin;
        AppId = appId;
        Tag = tag;
        Message = message;
    }

    public static LoginNotifyEvent Result(bool isLogin, uint appId, string tag, string message) => 
        new(isLogin, appId, tag, message);
}