namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysRequestInvitationEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string TargetUid { get; }
    
    public string InvitorUid { get; }

    private GroupSysRequestInvitationEvent(uint groupUin, string targetUid, string invitorUid) : base(0)
    {
        GroupUin = groupUin;
        TargetUid = targetUid;
        InvitorUid = invitorUid;
    }

    public static GroupSysRequestInvitationEvent Result(uint groupUin, string targetUid, string invitorUid) 
        => new(groupUin, targetUid, invitorUid);
}