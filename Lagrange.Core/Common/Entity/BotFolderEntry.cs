namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFolderEntry : IBotFSEntry
{
    public string FolderId { get; set; }
    
    public string ParentFolderId { get; set; }
    
    public string FolderName { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public DateTime ModifiedTime { get; set; }
    
    public uint CreatorUin { get; set; }
    
    public uint TotalFileCount { get; set; }

    internal BotFolderEntry(string folderId, string parentFolderId, string folderName, DateTime createTime, 
        DateTime modifiedTime, uint creatorUin, uint totalFileCount)
    {
        FolderId = folderId;
        ParentFolderId = parentFolderId;
        FolderName = folderName;
        CreateTime = createTime;
        ModifiedTime = modifiedTime;
        CreatorUin = creatorUin;
        TotalFileCount = totalFileCount;
    }
}