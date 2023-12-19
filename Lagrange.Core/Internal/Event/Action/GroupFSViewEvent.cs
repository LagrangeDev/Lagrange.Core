namespace Lagrange.Core.Internal.Event.Action;

internal abstract class GroupFSViewEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    protected GroupFSViewEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
    }

    protected GroupFSViewEvent(int resultCode) : base(resultCode) { }
}