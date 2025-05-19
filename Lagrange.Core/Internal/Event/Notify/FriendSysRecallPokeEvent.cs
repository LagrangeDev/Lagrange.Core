namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysRecallPokeEvent : ProtocolEvent
{
    public string PeerUid { get; }

    public string OperatorUid { get; }

    public ulong TipsSeqId { get; set; }

    private FriendSysRecallPokeEvent(string peerUid, string operatorUid, ulong tipsSeqId) : base(0)
    {
        PeerUid = peerUid;
        OperatorUid = operatorUid;
        TipsSeqId = tipsSeqId;
    }

    public static FriendSysRecallPokeEvent Result(string peerUid, string operatorUid, ulong tipsSeqId)
        => new(peerUid, operatorUid, tipsSeqId);
}
