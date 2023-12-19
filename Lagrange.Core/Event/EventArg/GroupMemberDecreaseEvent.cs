namespace Lagrange.Core.Event.EventArg;

public class GroupMemberDecreaseEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint MemberUin { get; }
    
    public uint? OperatorUin { get; }
    
    public GroupMemberDecreaseEvent(uint groupUin, uint memberUin, uint? operatorUin)
    {
        GroupUin = groupUin;
        MemberUin = memberUin;
        OperatorUin = operatorUin;
        
        EventMessage = $"{nameof(GroupMemberDecreaseEvent)}: {GroupUin} | {MemberUin} | {OperatorUin}";
    }
}