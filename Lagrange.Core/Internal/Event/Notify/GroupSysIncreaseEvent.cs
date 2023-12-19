namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysIncreaseEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string MemberUid { get; }
    
    public string? InvitorUid { get; }
    
    private GroupSysIncreaseEvent(uint groupUin, string memberUid, string? invitorUid) : base(0)
    {
        GroupUin = groupUin;
        MemberUid = memberUid;
        InvitorUid = invitorUid;
    }
    
    public static GroupSysIncreaseEvent Result(uint groupUin, string uid, string? invitorUid) => new(groupUin, uid, invitorUid);
}