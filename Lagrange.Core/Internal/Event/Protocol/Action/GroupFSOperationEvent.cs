namespace Lagrange.Core.Internal.Event.Protocol.Action;

internal abstract class GroupFSOperationEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    protected GroupFSOperationEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
    }

    protected GroupFSOperationEvent(int resultCode) : base(resultCode) { }
}