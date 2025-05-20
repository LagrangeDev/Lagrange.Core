namespace Lagrange.Core.Event.EventArg;

public class FriendPokeEvent : EventBase
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

    public FriendPokeEvent(uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl, ulong peerUin, ulong messageSequence, ulong messageTime, ulong tipsSeqId)
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

        EventMessage = $"{nameof(FriendPokeEvent)}: OperatorUin: {OperatorUin} | TargetUin: {TargetUin} | Action: {Action} | Suffix: {Suffix} | ActionImgUrl: {ActionImgUrl}";
    }
}
