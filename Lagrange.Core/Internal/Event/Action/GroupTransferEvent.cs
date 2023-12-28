namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupTransferEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    public string SourceUid { get; }
    public string TargetUid { get; }

    private GroupTransferEvent(uint groupUin, string sourceUid, string targetUid) : base(true)
    {
        GroupUin = groupUin;
        SourceUid = sourceUid;
        TargetUid = targetUid;
    }

    private GroupTransferEvent(int resultCode) : base(resultCode) { }

    public static GroupTransferEvent Create(uint groupUin, string sourceUid, string targetUid)
        => new(groupUin, sourceUid, targetUid);

    public static GroupTransferEvent Result(int resultCode) => new(resultCode);
}