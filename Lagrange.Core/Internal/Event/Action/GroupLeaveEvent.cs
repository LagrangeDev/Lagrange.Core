namespace Lagrange.Core.Internal.Event.Action;

internal class GroupLeaveEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    private GroupLeaveEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
    }

    private GroupLeaveEvent(int resultCode) : base(resultCode) { }

    public static GroupLeaveEvent Create(uint groupUin) => new(groupUin);
    
    public static GroupLeaveEvent Result(int resultCode) => new(resultCode);
}