namespace Lagrange.Core.Event.EventArg;

public class GroupNameChangeEvent : EventBase
{
    public uint GroupUin { get; }

    public string Name { get; }

    public GroupNameChangeEvent(uint groupUin, string name)
    {
        GroupUin = groupUin;
        Name = name;

        EventMessage = $"{nameof(GroupNameChangeEvent)}:  GroupUin: {GroupUin} | Name: {Name}";
    }
}