namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupFSRenameEvent : GroupFSOperationEvent
{
    public string FileId { get; set; }
    
    public string ParentDirectory { get; set; }
    
    public string TargetDirectory { get; set; }
    
    private GroupFSRenameEvent(uint groupUin, string fileId, string parentDirectory, string targetDirectory) : base(groupUin)
    {
        FileId = fileId;
        ParentDirectory = parentDirectory;
        TargetDirectory = targetDirectory;
    }

    private GroupFSRenameEvent(int resultCode) : base(resultCode) { }
    
    public static GroupFSRenameEvent Create(uint groupUin, string fileId, string parentDirectory, string targetDirectory)
        => new(groupUin, fileId, parentDirectory, targetDirectory);

    public static GroupFSRenameEvent Result(int resultCode)
        => new(resultCode);
}