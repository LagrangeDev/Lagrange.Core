namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysRequestJoinEvent : ProtocolEvent
{
    public string TargetUid { get; }
    
    public uint GroupUin { get; }

    private GroupSysRequestJoinEvent(uint groupUin, string targetUid) : base(0)
    {
        TargetUid = targetUid;
        GroupUin = groupUin;
    }

    public static GroupSysRequestJoinEvent Result(uint groupUin, string targetUid) => new(groupUin, targetUid);
}