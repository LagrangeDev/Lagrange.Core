namespace Lagrange.Core.Internal.Event.Message;

internal class PokeFriendEvent : ProtocolEvent
{
    public uint FriendUin { get; }

    protected PokeFriendEvent(uint friendUin) : base(true)
    {
        FriendUin = friendUin;
    }

    protected PokeFriendEvent(int resultCode) : base(resultCode) { }

    public static PokeFriendEvent Create(uint friendUin) => new(friendUin);
    
    public static PokeFriendEvent Result(int resultCode) => new(resultCode);
}