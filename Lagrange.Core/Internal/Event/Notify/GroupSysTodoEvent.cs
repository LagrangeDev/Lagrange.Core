namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysTodoEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public string OperatorUid { get; }

    private GroupSysTodoEvent(uint groupUin, string operatorUid) : base(0)
    {
        GroupUin = groupUin;
        OperatorUid = operatorUid;
    }

    public static GroupSysTodoEvent Result(uint groupUin, string operatorUid)
        => new(groupUin, operatorUid);
}