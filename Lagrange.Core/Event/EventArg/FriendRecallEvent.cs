namespace Lagrange.Core.Event.EventArg;

public class FriendRecallEvent : EventBase
{
    public uint FriendUin { get; }
    
    public uint Sequence { get; }
    
    public uint Time { get; }
    
    public uint Random { get; }

    public string Tip { get; }

    public FriendRecallEvent(uint friendUin, uint sequence, uint time, uint random, string tip)
    {
        FriendUin = friendUin;
        Sequence = sequence;
        Time = time;
        Random = random;
        Tip = tip;

        EventMessage = $"{nameof(FriendRecallEvent)}: {FriendUin} | ({Sequence} | {Time} | {Random} | {Tip})";
    }
}