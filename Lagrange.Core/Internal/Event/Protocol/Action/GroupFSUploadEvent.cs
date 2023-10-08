namespace Lagrange.Core.Internal.Event.Protocol.Action;

internal class GroupFSUploadEvent : GroupFSOperationEvent
{
    public GroupFSUploadEvent(uint groupUin) : base(groupUin)
    {
    }

    public GroupFSUploadEvent(int resultCode) : base(resultCode)
    {
    }
}