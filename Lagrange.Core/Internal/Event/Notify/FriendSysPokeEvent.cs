namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysPokeEvent : ProtocolEvent
{
    public uint FriendUin { get; }

    private FriendSysPokeEvent(uint friendUin) : base(0)
    {
        FriendUin = friendUin;
    }

    public static FriendSysPokeEvent Result(uint friendUin) => new(friendUin);
}