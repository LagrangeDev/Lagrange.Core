using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class FileUploadEvent : ProtocolEvent
{
    public string TargetUid { get; }
    
    public FileEntity Entity { get; }
    
    public string FileId { get; }
    
    public byte[] UploadKey { get; }
    
    public string Ip { get; }
    
    public uint Port { get; }
    
    public string Addon { get; }

    private FileUploadEvent(string targetUid, FileEntity entity) : base(true)
    {
        TargetUid = targetUid;
        Entity = entity;
    }

    private FileUploadEvent(int resultCode, string fileId, byte[] uploadKey, string ip, uint port, string addon) : base(resultCode)
    {
        FileId = fileId;
        UploadKey = uploadKey;
        Ip = ip;
        Port = port;
        Addon = addon;
    }

    public static FileUploadEvent Create(string targetUid, FileEntity entity) => new(targetUid, entity);

    public static FileUploadEvent Result(int resultCode, string fileId, byte[] uploadKey, string ip, uint port, string addon) 
        => new(resultCode, fileId, uploadKey, ip, port, addon);
}