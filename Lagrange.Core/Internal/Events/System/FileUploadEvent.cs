using Lagrange.Core.Events;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Events.System;

internal class FileUploadEventReq(string targetUid, Stream fileStream, string fileName, byte[] md510m) : ProtocolEvent
{
    public string TargetUid { get; } = targetUid;

    public Stream FileStream { get; } = fileStream;

    public string FileName { get; } = fileName;

    public byte[] FileMd5 { get; } = fileStream.Md5();
    
    public byte[] File10MMd5 { get; } = md510m;
    
    public byte[] FileSha1 { get; } = fileStream.Sha1();
}

internal class FileUploadEventResp(bool isExist, string fileId, byte[] uploadKey, List<(string, uint)> rtpMediaPlatformUploadAddress, string crcMedia) : ProtocolEvent
{
    public bool IsExist { get; } = isExist;

    public string FileId { get; } = fileId;

    public byte[] UploadKey { get; } = uploadKey;
    
    public List<(string, uint)> RtpMediaPlatformUploadAddress { get; } = rtpMediaPlatformUploadAddress;

    public string CrcMedia { get; } = crcMedia;
}