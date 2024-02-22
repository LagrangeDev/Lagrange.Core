namespace Lagrange.Core.Event.EventArg;

public class GroupRecallEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint Uin { get; }
    
    public uint Sequence { get; }
    
    public uint Time { get; }
    
    public uint Random { get; }

    public GroupRecallEvent(uint groupUin, uint uin, uint sequence, uint time, uint random)
    {
        GroupUin = groupUin;
        Uin = uin;
        Sequence = sequence;
        Time = time;
        Random = random;
        
        EventMessage = $"{nameof(GroupRecallEvent)}: {GroupUin} | {Uin} | ({Sequence} | {Time} | {Random})";
    }
}