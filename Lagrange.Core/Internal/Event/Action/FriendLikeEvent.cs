namespace Lagrange.Core.Internal.Event.Action;

internal class FriendLikeEvent : ProtocolEvent
{
    public string TargetUid { get; } = "";

    private FriendLikeEvent(string targetUid) : base(true)
    {
        TargetUid = targetUid;
    }

    private FriendLikeEvent(int resultCode) : base(resultCode) { }
    
    public static FriendLikeEvent Create(string targetUid) => new(targetUid);
    
    public static FriendLikeEvent Result(int resultCode) => new(resultCode);
}