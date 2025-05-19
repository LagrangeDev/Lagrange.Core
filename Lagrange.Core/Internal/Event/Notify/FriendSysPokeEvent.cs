namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysPokeEvent : ProtocolEvent
{
    public uint OperatorUin { get; }

    public uint TargetUin { get; }

    public string Action { get; }

    public string Suffix { get; }

    public string ActionImgUrl { get; }

    public ulong PeerUin { get; }

    public ulong MessageSequence { get; }

    public ulong MessageTime { get; }

    public ulong TipsSeqId { get; }

    private FriendSysPokeEvent(uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl, ulong peerUin, ulong messageSequence, ulong messageTime, ulong tipsSeqId) : base(0)
    {
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImgUrl = actionImgUrl;
        PeerUin = peerUin;
        MessageSequence = messageSequence;
        MessageTime = messageTime;
        TipsSeqId = tipsSeqId;
    }

    public static FriendSysPokeEvent Result(uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl, ulong peerUin, ulong messageSequence, ulong messageTime, ulong tipsSeqId)
        => new(operatorUin, targetUin, action, suffix, actionImgUrl, peerUin, messageSequence, messageTime, tipsSeqId);
}
