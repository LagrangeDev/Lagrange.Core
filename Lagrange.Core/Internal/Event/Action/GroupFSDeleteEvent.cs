namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupFSDeleteEvent : GroupFSOperationEvent
{
    public string FileId { get; set; }
    
    public string RetMsg { get; set; } = string.Empty;
    
    public GroupFSDeleteEvent(uint groupUin, string fileId) : base(groupUin)
    {
        FileId = fileId;
    }

    public GroupFSDeleteEvent(int resultCode, string retMsg) : base(resultCode)
    {
        RetMsg = retMsg;
    }
    
    public static GroupFSDeleteEvent Create(uint groupUin, string fileId)
        => new(groupUin, fileId);

    public static GroupFSDeleteEvent Result(int resultCode, string retMsg) => new(resultCode, retMsg);
}