namespace Lagrange.Core.Event.EventArg;

public class GroupRecallPokeEvent : EventBase
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public ulong TipsSeqId { get; set; }

    public GroupRecallPokeEvent(uint groupUin, uint operatorUin, ulong tipsSeqId)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TipsSeqId = tipsSeqId;

        EventMessage = $"[GroupRecallPoke] Group: {GroupUin} | Operator: {OperatorUin} | TipsSeqId: {TipsSeqId}";
    }
}