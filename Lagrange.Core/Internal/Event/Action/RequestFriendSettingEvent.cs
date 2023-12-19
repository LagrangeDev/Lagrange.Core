namespace Lagrange.Core.Internal.Event.Action;

internal class RequestFriendSettingEvent : ProtocolEvent
{
    public uint TargetUin { get; set; }

    private RequestFriendSettingEvent(uint targetUin) : base(true)
    {
        TargetUin = targetUin;
    }
    
    private RequestFriendSettingEvent(int resultCode) : base(resultCode) { }
    
    public static RequestFriendSettingEvent Create(uint targetUin) => new(targetUin);
    
    public static RequestFriendSettingEvent Result(int resultCode) => new(resultCode);
}