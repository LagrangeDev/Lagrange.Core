namespace Lagrange.Core.Event.EventArg;

public class GroupMemberIncreaseEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint MemberUin { get; }
    
    public uint? InvitorUin { get; }
    
    public EventType Type { get; }
    
    public GroupMemberIncreaseEvent(uint groupUin, uint memberUin, uint? invitorUin, uint type)
    {
        GroupUin = groupUin;
        MemberUin = memberUin;
        InvitorUin = invitorUin;
        Type = (EventType)type;

        EventMessage = $"{nameof(GroupMemberIncreaseEvent)}: {GroupUin} | {MemberUin} | {InvitorUin} | {Type}";
    }

    public enum EventType : uint
    {
        Approve = 130,
        Invite = 131
    }
}
