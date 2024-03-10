namespace Lagrange.Core.Internal.Event.Message;

internal class GroupPokeEvent : FriendPokeEvent
{
    public uint GroupUin { get; }
    
    private GroupPokeEvent(uint friendUin, uint groupUin) : base(friendUin)
    {
        GroupUin = groupUin;
    }

    private GroupPokeEvent(int resultCode) : base(resultCode) { }
    
    public static GroupPokeEvent Create(uint friendUin, uint groupUin) => new(friendUin, groupUin);
}