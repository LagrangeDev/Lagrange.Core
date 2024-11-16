namespace Lagrange.Core.Internal.Event.Action;

internal class FetchPinsEvent : ProtocolEvent
{
    internal List<string> FriendUids { get; set; }

    public List<uint> FriendUins { get; set; }

    public List<uint> GroupUins { get; set; }

    public string Message { get; set; }

    protected FetchPinsEvent() : base(true)
    {
        FriendUids = new();
        FriendUins = new();
        GroupUins = new();
        Message = string.Empty;
    }

    protected FetchPinsEvent(List<string> friendUids, List<uint> groupUins) : base(0)
    {
        FriendUids = friendUids;
        FriendUins = new();
        GroupUins = groupUins;
        Message = string.Empty;
    }

    protected FetchPinsEvent(int retcode, string message) : base(retcode)
    {
        FriendUids = new();
        FriendUins = new();
        GroupUins = new();
        Message = string.Empty;
    }

    public static FetchPinsEvent Create() => new();

    public static FetchPinsEvent Result(List<string> friendUids, List<uint> groupUins) => new(friendUids, groupUins);

    public static FetchPinsEvent Result(int retcode, string message) => new(retcode, message);
}