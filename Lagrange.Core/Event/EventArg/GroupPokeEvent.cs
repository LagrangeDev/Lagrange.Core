namespace Lagrange.Core.Event.EventArg;

public class GroupPokeEvent : EventBase
{
    public uint FriendUin { get; }

    public uint GroupUin { get; }

    public GroupPokeEvent(uint friendUin, uint groupUin)
    {
        FriendUin = friendUin;
        GroupUin = groupUin;
        
        EventMessage = $"{nameof(GroupPokeEvent)}: {FriendUin} {GroupUin}";
    }
}