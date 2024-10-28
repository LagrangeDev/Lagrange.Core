namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysMemberEnterEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint GroupMemberUin { get; }

    public uint StyleId { get; }

    private GroupSysMemberEnterEvent(uint groupUin, uint groupMemberUin, uint styleId) : base(0)
    {
        GroupUin = groupUin;
        GroupMemberUin = groupMemberUin;
        StyleId = styleId;
    }

    public static GroupSysMemberEnterEvent Result(uint groupUin, uint groupMemberUin, uint styleId) =>
        new(groupUin, groupMemberUin, styleId);
}