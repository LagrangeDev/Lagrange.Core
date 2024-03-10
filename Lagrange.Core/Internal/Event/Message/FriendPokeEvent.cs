namespace Lagrange.Core.Internal.Event.Message;

internal class FriendPokeEvent : ProtocolEvent
{
    public uint FriendUin { get; }

    protected FriendPokeEvent(uint friendUin) : base(true)
    {
        FriendUin = friendUin;
    }

    protected FriendPokeEvent(int resultCode) : base(resultCode) { }

    public static FriendPokeEvent Create(uint friendUin) => new(friendUin);
    
    public static FriendPokeEvent Result(int resultCode) => new(resultCode);
}