namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysUniqueTitleChangeEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint TargetUin { get; }

    public string Title { get; }

    private GroupSysUniqueTitleChangeEvent(uint groupUin, uint targetUin, string title) : base(0)
    {
        GroupUin = groupUin;
        TargetUin = targetUin;
        Title = title;
    }

    public static GroupSysUniqueTitleChangeEvent Result(uint groupUin, uint targetUin, string title) => new(groupUin, targetUin, title);
}