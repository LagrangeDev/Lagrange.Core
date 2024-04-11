namespace Lagrange.Core.Internal.Event.Message;

internal class PokeGroupMemberEvent : PokeFriendEvent
{
    public uint GroupUin { get; }
    
    private PokeGroupMemberEvent(uint friendUin, uint groupUin) : base(friendUin)
    {
        GroupUin = groupUin;
    }

    private PokeGroupMemberEvent(int resultCode) : base(resultCode) { }
    
    public static PokeGroupMemberEvent Create(uint friendUin, uint groupUin) => new(friendUin, groupUin);
}