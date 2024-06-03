namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysInviteEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public string InvitorUid { get; set; }

    private GroupSysInviteEvent(uint groupUin, string invitorUid) : base(0)
    {
        GroupUin = groupUin;
        InvitorUid = invitorUid;
    }
    
    public static GroupSysInviteEvent Result(uint groupUin, string invitorUid) => new(groupUin, invitorUid);
}