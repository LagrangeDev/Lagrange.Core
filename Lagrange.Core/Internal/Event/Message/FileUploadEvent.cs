using System.Diagnostics.CodeAnalysis;
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class FileUploadEvent : ProtocolEvent
{
    public string TargetUid { get; }

    public FileEntity Entity { get; }

    public bool IsExist { get; }

    public string FileId { get; }

    public byte[] UploadKey { get; }

    public string Ip { get; }

    public uint Port { get; }

    public string Addon { get; }

    public string? Message { get; }

    [MemberNotNullWhen(false, nameof(Message))]
    public bool IsSuccess => ResultCode == 0;

    private FileUploadEvent(string targetUid, FileEntity entity) : base(true)
    {
        TargetUid = targetUid;
        Entity = entity;
    }

    private FileUploadEvent(int resultCode, string? message, bool isExist, string fileId, byte[] uploadKey, string ip, uint port, string addon) : base(resultCode)
    {
        Message = message;
        IsExist = isExist;
        FileId = fileId;
        UploadKey = uploadKey;
        Ip = ip;
        Port = port;
        Addon = addon;
    }

    public static FileUploadEvent Create(string targetUid, FileEntity entity) => new(targetUid, entity);

    public static FileUploadEvent Result(int resultCode, string? message, bool isExist, string fileId, byte[] uploadKey, string ip, uint port, string addon)
        => new(resultCode, message, isExist, fileId, uploadKey, ip, port, addon);
}