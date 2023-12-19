namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupFSDeleteEvent : GroupFSOperationEvent
{
    public string FileId { get; set; }
    
    public string ParentDirectory { get; set; }
    
    public GroupFSDeleteEvent(uint groupUin, string fileId, string parentDirectory) : base(groupUin)
    {
        FileId = fileId;
        ParentDirectory = parentDirectory;
    }

    public GroupFSDeleteEvent(int resultCode) : base(resultCode) { }
    
    public static GroupFSDeleteEvent Create(uint groupUin, string fileId, string parentDirectory)
        => new(groupUin, fileId, parentDirectory);

    public static GroupFSDeleteEvent Result(int resultCode)
        => new(resultCode);
}