namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupFSMoveEvent : GroupFSOperationEvent
{
    public string FileId { get; set; }
    
    public string ParentDirectory { get; set; }
    
    public string TargetDirectory { get; set; }
    
    private GroupFSMoveEvent(uint groupUin, string fileId, string parentDirectory, string targetDirectory) : base(groupUin)
    {
        FileId = fileId;
        ParentDirectory = parentDirectory;
        TargetDirectory = targetDirectory;
    }
    
    private GroupFSMoveEvent(int resultCode) : base(resultCode) { }

    public static GroupFSMoveEvent Create(uint groupUin, string fileId, string parentDirectory, string targetDirectory)
        => new(groupUin, fileId, parentDirectory, targetDirectory);

    public static GroupFSMoveEvent Result(int resultCode)
        => new(resultCode);
}