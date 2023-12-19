using Lagrange.Core.Message.Entity;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class GroupFSUploadEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    public FileEntity Entity { get; }

    private GroupFSUploadEvent(uint groupUin, FileEntity entity) : base(true)
    {
        GroupUin = groupUin;
        Entity = entity;
    }

    private GroupFSUploadEvent(int resultCode) : base(resultCode)
    {
    }

    public static GroupFSUploadEvent Create(uint groupUin, FileEntity entity) => new(groupUin, entity);
}