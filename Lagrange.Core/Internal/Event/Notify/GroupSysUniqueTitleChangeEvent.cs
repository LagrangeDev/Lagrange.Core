namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysUniqueTitleChangeEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint FriendUin { get; }

    public string Title { get; }

    private GroupSysUniqueTitleChangeEvent(uint groupUin, uint friendUin, string title) : base(0)
    {
        GroupUin = groupUin;
        FriendUin = friendUin;
        Title = title;
    }

    public static GroupSysUniqueTitleChangeEvent Result(uint groupUin, uint friendUin, string title) => new(groupUin, friendUin, title);
}