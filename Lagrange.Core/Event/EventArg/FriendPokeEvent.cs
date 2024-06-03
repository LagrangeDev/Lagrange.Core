namespace Lagrange.Core.Event.EventArg;

public class FriendPokeEvent : EventBase
{
    public uint FriendUin { get; }

    public FriendPokeEvent(uint friendUin)
    {
        FriendUin = friendUin;
        
        EventMessage = $"{nameof(FriendPokeEvent)}: {FriendUin}";
    }
}