namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysPokeEvent : ProtocolEvent
{
    public uint FriendUin { get; }

    public string Action { get; }

    public string Suffix { get; }

    public string ActionImage { get; }

    public bool Full { get; }

    private FriendSysPokeEvent(uint friendUin, string action, string suffix, string actionImage) : base(0)
    {
        FriendUin = friendUin;
        Action = action;
        Suffix = suffix;
        ActionImage = actionImage;
        Full = true;
    }

    public FriendSysPokeEvent(uint friendUin) : base(0)
    {
        FriendUin = friendUin;
        Full = false;
    }

    public static FriendSysPokeEvent Result(uint friendUin, string action, string suffix, string actionImage) => new(friendUin, action, suffix, actionImage);

    public static FriendSysPokeEvent Result(uint friendUin) => new(friendUin);
}