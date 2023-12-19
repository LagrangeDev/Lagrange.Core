namespace Lagrange.Core.Internal.Event.Action;

internal class RequestFriendSearchEvent : ProtocolEvent
{
    public uint TargetUin { get; }

    private RequestFriendSearchEvent(uint targetUin) : base(true)
    {
        TargetUin = targetUin;
    }
    
    private RequestFriendSearchEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static RequestFriendSearchEvent Create(uint targetUin) => new(targetUin);

    public static RequestFriendSearchEvent Result(int resultCode) => new(resultCode);
}