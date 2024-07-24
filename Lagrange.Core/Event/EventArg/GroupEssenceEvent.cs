namespace Lagrange.Core.Event.EventArg;

public class GroupEssenceEvent : EventBase
{
    public uint GroupUin { get; }

    public uint Sequence { get; }

    public uint Random { get; }

    public bool IsSet { get; } // 1 设置精华消息, 2 移除精华消息

    public uint FromUin { get; }

    public uint OperatorUin { get; }

    public GroupEssenceEvent(uint groupUin, uint sequence,uint random, uint setFlag, uint fromUin, uint operatorUin)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Random = random;
        IsSet = setFlag == 1;
        FromUin = fromUin;
        OperatorUin = operatorUin;

        EventMessage = $"{nameof(GroupEssenceEvent)}: {GroupUin} | {FromUin} | {OperatorUin} | ({Sequence}) | IsSet: {IsSet}";
    }
}
