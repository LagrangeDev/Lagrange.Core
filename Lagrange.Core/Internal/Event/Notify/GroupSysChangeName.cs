namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysNameChangeEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public string Name { get; }

    private GroupSysNameChangeEvent(uint groupUin, string name) : base(0)
    {
        GroupUin = groupUin;
        Name = name;
    }

    public static GroupSysNameChangeEvent Result(uint groupUin, string name)
        => new(groupUin, name);
}