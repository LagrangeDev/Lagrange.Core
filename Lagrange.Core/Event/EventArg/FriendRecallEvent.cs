namespace Lagrange.Core.Event.EventArg;

public class FriendRecallEvent : EventBase
{
    public uint FriendUin { get; }

    public uint ClientSequence { get; }

    public uint Time { get; }

    public uint Random { get; }

    public string Tip { get; }

    public FriendRecallEvent(uint friendUin, uint clientSequence, uint time, uint random, string tip)
    {
        FriendUin = friendUin;
        ClientSequence = clientSequence;
        Time = time;
        Random = random;
        Tip = tip;

        EventMessage = $"{nameof(FriendRecallEvent)}: {FriendUin} | ({ClientSequence} | {Time} | {Random} | {Tip})";
    }
}