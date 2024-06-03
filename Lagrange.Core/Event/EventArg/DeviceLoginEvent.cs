namespace Lagrange.Core.Event.EventArg;

public class DeviceLoginEvent : EventBase
{
    public bool IsLogin { get; }
    
    public uint AppId { get; }
    
    public string Tag { get; }
    
    public string Message { get; }
    

    public DeviceLoginEvent(bool isLogin, uint appId, string tag, string message)
    {
        IsLogin = isLogin;
        AppId = appId;
        Tag = tag;
        Message = message;

        EventMessage = $"[{nameof(DeviceLoginEvent)}]: {Tag} | {Message}, AppID: {AppId}, IsLogin: {IsLogin}";
    }
}