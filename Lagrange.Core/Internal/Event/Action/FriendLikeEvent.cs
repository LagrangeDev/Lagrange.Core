namespace Lagrange.Core.Internal.Event.Action;

internal class FriendLikeEvent : ProtocolEvent
{
    public string TargetUid { get; } = "";
    
    public uint Count { get; }

    private FriendLikeEvent(string targetUid, uint count) : base(true)
    {
        TargetUid = targetUid;
        Count = count;
    }

    private FriendLikeEvent(int resultCode) : base(resultCode) { }
    
    public static FriendLikeEvent Create(string targetUid, uint count) => new(targetUid, count);
    
    public static FriendLikeEvent Result(int resultCode) => new(resultCode);
}