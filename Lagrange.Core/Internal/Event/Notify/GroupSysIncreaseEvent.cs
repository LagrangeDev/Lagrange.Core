namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysIncreaseEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string MemberUid { get; }
    
    public string? InvitorUid { get; }
    
    public uint Type { get; }
    
    private GroupSysIncreaseEvent(uint groupUin, string memberUid, string? invitorUid, uint type) : base(0)
    {
        GroupUin = groupUin;
        MemberUid = memberUid;
        InvitorUid = invitorUid;
        Type = type;
    }

    public static GroupSysIncreaseEvent Result(uint groupUin, string uid, string? invitorUid, uint type) =>
        new(groupUin, uid, invitorUid, type);
}