using System;

namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFileEntry : IBotFSEntry
{
    internal string FileId { get; }
    
    public string FileName { get; }
    
    public string ParentDirectory { get; }
    
    public ulong FileSize { get; }
    
    public DateTimeOffset ExpireTime { get; }
    
    public DateTimeOffset ModifiedTime { get; }
    
    public uint UploaderUin { get; }
    
    public DateTimeOffset UploadedTime { get; }
    
    public uint DownloadedTimes { get; }

    internal BotFileEntry(string fileId, string fileName, string parentDirectory, ulong fileSize, 
        DateTimeOffset expireTime, DateTimeOffset modifiedTime, uint uploaderUin, DateTimeOffset uploadedTime, uint downloadedTimes)
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