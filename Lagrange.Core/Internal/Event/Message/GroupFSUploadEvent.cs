using System.Diagnostics.CodeAnalysis;
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

    public string? Message { get; }

    [MemberNotNullWhen(false, nameof(Message))]
    public bool IsSuccess => ResultCode == 0;

    private GroupFSUploadEvent(uint groupUin, string targetDirectory, FileEntity entity) : base(true)
    {
        GroupUin = groupUin;
        TargetDirectory = targetDirectory;
        Entity = entity;
    }

    private GroupFSUploadEvent(int resultCode, string? message, bool isExist, string fileId, byte[] uploadKey, byte[] checkKey, string ip, uint port) : base(resultCode)
    {
        Message = message;
        IsExist = isExist;
        FileId = fileId;
        UploadKey = uploadKey;
        CheckKey = checkKey;
        Ip = ip;
        Port = port;
    }

    public static GroupFSUploadEvent Create(uint groupUin, string targetDirectory, FileEntity entity)
        => new(groupUin, targetDirectory, entity);

    public static GroupFSUploadEvent Result(int resultCode, string? message, bool isExist, string fileId, byte[] uploadKey, byte[] checkKey, string ip, uint port)
        => new(resultCode, message, isExist, fileId, uploadKey, checkKey, ip, port);
}