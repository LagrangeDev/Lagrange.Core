namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysDecreaseEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string MemberUid { get; }
    
    public string? OperatorUid { get; }
    
    private GroupSysDecreaseEvent(uint groupUin, string memberUid, string? operatorUid) : base(0)
    {
        GroupUin = groupUin;
        MemberUid = memberUid;
        OperatorUid = operatorUid;
    }
    
    public static GroupSysDecreaseEvent Result(uint groupUin, string uid, string? operatorUid) => new(groupUin, uid, operatorUid);
}