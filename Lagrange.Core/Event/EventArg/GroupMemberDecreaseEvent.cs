namespace Lagrange.Core.Event.EventArg;

public class GroupMemberDecreaseEvent : EventBase
{
    public uint GroupUin { get; }

    public uint MemberUin { get; }

    public uint? OperatorUin { get; }

    public EventType Type { get; }

    public GroupMemberDecreaseEvent(uint groupUin, uint memberUin, uint? operatorUin, uint type)
    {
        GroupUin = groupUin;
        MemberUin = memberUin;
        OperatorUin = operatorUin;
        Type = (EventType)type;

        EventMessage = $"{nameof(GroupMemberDecreaseEvent)}: {GroupUin} | {MemberUin} | {OperatorUin} | {Type}";
    }

    public enum EventType : uint
    {
        KickMe = 3,
        Leave = 130,
        Kick = 131
    }
}