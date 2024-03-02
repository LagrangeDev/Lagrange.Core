namespace Lagrange.Core.Internal.Event.Message;

internal class GroupSendFileEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string FileKey { get; } = string.Empty;

    private GroupSendFileEvent(uint groupUin, string fileKey) : base(true)
    {
        GroupUin = groupUin;
        FileKey = fileKey;
    }

    private GroupSendFileEvent(int resultCode) : base(resultCode) { }

    public static GroupSendFileEvent Create(uint groupUin, string fileKey) => new(groupUin, fileKey);

    public static GroupSendFileEvent Result(int resultCode) => new(resultCode);
}