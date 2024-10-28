namespace Lagrange.Core.Event.EventArg;

public class GroupMemberEnterEvent : EventBase
{
    public uint GroupUin { get; }

    public uint GroupMemberUin { get; }

    public uint StyleId { get; }

    internal GroupMemberEnterEvent(uint groupUin, uint groupMemberUin, uint styleId)
    {
        GroupUin = groupUin;
        GroupMemberUin = groupMemberUin;
        StyleId = styleId;

        EventMessage = $"[{nameof(GroupMemberEnterEvent)}]: {GroupMemberUin} Enter {GroupUin} | {StyleId}";
    }
}