namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFileEntry : IBotFSEntry
{
    public string FileId { get; }
    
    public string FileName { get; }
    
    public string ParentDirectory { get; }
    
    public ulong FileSize { get; }
    
    public DateTime ExpireTime { get; }
    
    public DateTime ModifiedTime { get; }
    
    public uint UploaderUin { get; }
    
    public DateTime UploadedTime { get; }
    
    public uint DownloadedTimes { get; }

    internal BotFileEntry(string fileId, string fileName, string parentDirectory, ulong fileSize, 
        DateTime expireTime, DateTime modifiedTime, uint uploaderUin, DateTime uploadedTime, uint downloadedTimes)
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