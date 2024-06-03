namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysDecreaseEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string MemberUid { get; }
    
    public string? OperatorUid { get; }
    
    public uint Type { get; }
    
    private GroupSysDecreaseEvent(uint groupUin, string memberUid, string? operatorUid, uint type) : base(0)
    {
        GroupUin = groupUin;
        MemberUid = memberUid;
        OperatorUid = operatorUid;
        Type = type;
    }

    public static GroupSysDecreaseEvent Result(uint groupUin, string uid, string? operatorUid, uint type) =>
        new(groupUin, uid, operatorUid, type);
}