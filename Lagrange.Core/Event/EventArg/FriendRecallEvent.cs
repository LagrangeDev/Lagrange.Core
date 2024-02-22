namespace Lagrange.Core.Event.EventArg;

public class FriendRecallEvent : EventBase
{
    public uint FriendUin { get; }
    
    public uint Sequence { get; }
    
    public uint Time { get; }
    
    public uint Random { get; }

    public FriendRecallEvent(uint friendUin, uint sequence, uint time, uint random)
    {
        FriendUin = friendUin;
        Sequence = sequence;
        Time = time;
        Random = random;
        
        EventMessage = $"{nameof(FriendRecallEvent)}: {FriendUin} | ({Sequence} | {Time} | {Random})";
    }
}