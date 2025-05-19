namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysPokeEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public uint TargetUin { get; }

    public string Action { get; }

    public string Suffix { get; }

    public string ActionImgUrl { get; }

    public ulong MessageSequence { get; set; }

    public ulong MessageTime { get; set; }

    public ulong TipsSeqId { get; set; }

    private GroupSysPokeEvent(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl, ulong messageSequence, ulong messageTime, ulong tipsSeqId) : base(0)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImgUrl = actionImgUrl;
        MessageSequence = messageSequence;
        MessageTime = messageTime;
        TipsSeqId = tipsSeqId;
    }

    public static GroupSysPokeEvent Result(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl, ulong messageSequence, ulong messageTime, ulong tipsSeqId)
        => new(groupUin, operatorUin, targetUin, action, suffix, actionImgUrl, messageSequence, messageTime, tipsSeqId);
}
