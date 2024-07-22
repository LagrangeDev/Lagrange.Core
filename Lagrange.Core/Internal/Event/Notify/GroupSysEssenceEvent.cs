namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysEssenceEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint Sequence { get; }

    public uint Random { get; }

    public uint SetFlag { get; } // 1 设置精华消息, 2 移除精华消息

    public uint FromUin { get; }

    public uint OperatorUin { get; }

    private GroupSysEssenceEvent(uint groupUin, uint sequence, uint random, uint setFlag, uint fromUin, uint operatorUin) : base(0)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Random = random;
        SetFlag = setFlag;
        FromUin = fromUin;
        OperatorUin = operatorUin;
    }

    public static GroupSysEssenceEvent Result(uint groupUin, uint sequence,uint random, uint setFlag, uint fromUin, uint operatorUin)
        => new(groupUin, sequence, random, setFlag, fromUin, operatorUin);
}