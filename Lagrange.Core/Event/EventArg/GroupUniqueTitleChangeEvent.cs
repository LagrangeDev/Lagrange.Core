namespace Lagrange.Core.Event.EventArg;

public class GroupUniqueTitleChangeEvent : EventBase
{
    public uint GroupUin { get; }

    public uint TargetUin { get; }

    public string Title { get; }

    public GroupUniqueTitleChangeEvent(uint groupUin, uint targetUin, string title)
    {
        GroupUin = groupUin;
        TargetUin = targetUin;
        Title = title;

        EventMessage = $"{nameof(GroupUniqueTitleChangeEvent)}: {GroupUin} | {TargetUin} | {Title}";
    }
}