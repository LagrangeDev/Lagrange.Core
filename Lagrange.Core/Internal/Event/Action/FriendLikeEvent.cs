namespace Lagrange.Core.Internal.Event.Action;

internal class FriendLikeEvent : ProtocolEvent
{
    public string TargetUid { get; } = "";

    public uint? TargetUin { get; set; }

    public uint Count { get; }

    public string Error { get; set; } = "";

    private FriendLikeEvent(string targetUid, uint count) : base(true)
    {
        TargetUid = targetUid;
        Count = count;
    }

    private FriendLikeEvent(uint targetUin, uint count) : base(true)
    {
        TargetUin = targetUin;
        Count = count;
    }

    private FriendLikeEvent(int resultCode) : base(resultCode) { }

    private FriendLikeEvent(int resultCode, string errormsg) : base(resultCode)
    {
        Error = errormsg;
    }

    public static FriendLikeEvent Create(uint targetUin, uint count) => new(targetUin, count);

    public static FriendLikeEvent Result(int resultCode, string errormsg) => new(resultCode, errormsg);
}