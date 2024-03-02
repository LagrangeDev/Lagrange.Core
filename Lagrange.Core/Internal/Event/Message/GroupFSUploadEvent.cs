using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class GroupFSUploadEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public FileEntity Entity { get; }
    
    public string FileId { get; }
    
    public byte[] UploadKey { get; }
    
    public byte[] CheckKey { get; }
    
    public string Ip { get; }
    
    public uint Port { get; }

    private GroupFSUploadEvent(uint groupUin, FileEntity entity) : base(true)
    {
        GroupUin = groupUin;
        Entity = entity;
    }

    private GroupFSUploadEvent(int resultCode, string fileId, byte[] uploadKey, byte[] checkKey, string ip, uint port) : base(resultCode)
    {
        FileId = fileId;
        UploadKey = uploadKey;
        CheckKey = checkKey;
        Ip = ip;
        Port = port;
    }

    public static GroupFSUploadEvent Create(uint groupUin, FileEntity entity) => new(groupUin, entity);

    public static GroupFSUploadEvent Result(int resultCode, string fileId, byte[] uploadKey, byte[] checkKey, string ip, uint port) 
        => new(resultCode, fileId, uploadKey, checkKey, ip, port);
}