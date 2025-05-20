namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysRecallPokeEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public string OperatorUid { get; }

    public ulong TipsSeqId { get; set; }

    private GroupSysRecallPokeEvent(uint groupUin, string operatorUid, ulong tipsSeqId) : base(0)
    {
        GroupUin = groupUin;
        OperatorUid = operatorUid;
        TipsSeqId = tipsSeqId;
    }

    public static GroupSysRecallPokeEvent Result(uint groupUin, string operatorUid, ulong tipsSeqId)
        => new(groupUin, operatorUid, tipsSeqId);
}
