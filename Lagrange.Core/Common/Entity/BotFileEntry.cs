namespace Lagrange.Core.Common.Entity;

/// <summary>
/// Indicates an entry in a group file system listing.
/// </summary>
public interface IBotFSEntry;

[Serializable]
public class BotFileEntry : IBotFSEntry
{
    public string FileId { get; }

    public string FileName { get; }

    public string ParentDirectory { get; }

    public ulong FileSize { get; }

    public long ExpireTime { get; }

    public long ModifiedTime { get; }

    public long UploaderUin { get; }

    public long UploadedTime { get; }

    public uint DownloadedTimes { get; }

    internal BotFileEntry(string fileId, string fileName, string parentDirectory, ulong fileSize, long expireTime, long modifiedTime, long uploaderUin, long uploadedTime, uint downloadedTimes)
    {
        FileId = fileId;
        FileName = fileName;
        ParentDirectory = parentDirectory;
        FileSize = fileSize;
        ExpireTime = expireTime;
        ModifiedTime = modifiedTime;
        UploaderUin = uploaderUin;
        UploadedTime = uploadedTime;
        DownloadedTimes = downloadedTimes;
    }
}

[Serializable]
public class BotFolderEntry : IBotFSEntry
{
    public string FolderId { get; }

    public string ParentFolderId { get; }

    public string FolderName { get; }

    public long CreateTime { get; }

    public long ModifiedTime { get; }

    public long CreatorUin { get; }

    public uint TotalFileCount { get; }

    internal BotFolderEntry(string folderId, string parentFolderId, string folderName, long createTime, long modifiedTime, long creatorUin, uint totalFileCount)
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
