namespace Lagrange.Core.Event.EventArg;

public class FriendRecallPokeEvent : EventBase
{
    public uint PeerUin { get; }

    public uint OperatorUin { get; }

    public ulong TipsSeqId { get; set; }

    public FriendRecallPokeEvent(uint groupUin, uint operatorUin, ulong tipsSeqId)
    {
        PeerUin = groupUin;
        OperatorUin = operatorUin;
        TipsSeqId = tipsSeqId;

        EventMessage = $"[FriendRecallPoke] Peer: {PeerUin} | Operator: {OperatorUin} | TipsSeqId: {TipsSeqId}";
    }
}