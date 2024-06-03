namespace Lagrange.Core.Internal.Event.System;

internal class SetFriendRequestEvent : ProtocolEvent
{
    public string TargetUid { get; } = string.Empty;
    
    public bool Accept { get; }

    private SetFriendRequestEvent(string targetUid, bool accept) : base(true)
    {
        TargetUid = targetUid;
        Accept = accept;
    }

    private SetFriendRequestEvent(int resultCode) : base(resultCode) { }

    public static SetFriendRequestEvent Create(string targetUid, bool accept) => new(targetUid, accept);
    
    public static SetFriendRequestEvent Result(int resultCode) => new(resultCode);
}