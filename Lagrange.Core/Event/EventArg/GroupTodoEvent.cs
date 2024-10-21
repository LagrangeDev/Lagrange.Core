namespace Lagrange.Core.Event.EventArg;

public class GroupTodoEvent : EventBase
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public GroupTodoEvent(uint groupUin, uint operatorUin)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;

        EventMessage = $"{nameof(GroupPokeEvent)}:  GroupUin: {GroupUin} | OperatorUin: {OperatorUin}";
    }
}
