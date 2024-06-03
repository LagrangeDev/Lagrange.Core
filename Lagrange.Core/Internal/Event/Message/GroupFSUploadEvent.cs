using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class GroupFSUploadEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public string TargetDirectory { get; }
    
    public FileEntity Entity { get; }

    public bool IsExist { get; }
    
    public string FileId { get; }
    
    public byte[] UploadKey { get; }
    
    public byte[] CheckKey { get; }
    
    public string Ip { get; }
    
    public uint Port { get; }

    private GroupFSUploadEvent(uint groupUin, string targetDirectory, FileEntity entity) : base(true)
    {
        GroupUin = groupUin;
        TargetDirectory = targetDirectory;
        Entity = entity;
    }

    private GroupFSUploadEvent(int resultCode, bool isExist, string fileId, byte[] uploadKey, byte[] checkKey, string ip, uint port) : base(resultCode)
    {
        IsExist = isExist;
        FileId = fileId;
        UploadKey = uploadKey;
        CheckKey = checkKey;
        Ip = ip;
        Port = port;
    }

    public static GroupFSUploadEvent Create(uint groupUin, string targetDirectory, FileEntity entity) 
        => new(groupUin, targetDirectory, entity);

    public static GroupFSUploadEvent Result(int resultCode, bool isExist, string fileId, byte[] uploadKey, byte[] checkKey, string ip, uint port) 
        => new(resultCode, isExist, fileId, uploadKey, checkKey, ip, port);
}