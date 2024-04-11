namespace Lagrange.Core.Event.EventArg;

public class FriendPokeEvent : EventBase
{
    public uint FriendUin { get; }

    public string Action { get; }

    public string Suffix { get; }

    public string ActionImage { get; }

    public FriendPokeEvent(uint friendUin)
    {
        FriendUin = friendUin;

        EventMessage = $"{nameof(FriendPokeEvent)}: {FriendUin}";
    }

    public FriendPokeEvent(uint friendUin, string action, string suffix, string actionImage)
    {
        FriendUin = friendUin;
        Action = action;
        Suffix = suffix;
        ActionImage = actionImage;
        
        EventMessage = $"{nameof(FriendPokeEvent)}: {FriendUin} {Action} You {Suffix} | {ActionImage}";
    }
}