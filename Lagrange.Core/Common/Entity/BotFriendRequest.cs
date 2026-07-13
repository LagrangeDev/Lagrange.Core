namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFriendRequest
{
    public BotFriendRequest(
        long targetUin,
        long sourceUin,
        uint eventState,
        string comment,
        string source,
        uint time)
    {
        TargetUin = targetUin;
        SourceUin = sourceUin;
        EventState = (State)eventState;
        Comment = comment;
        Source = source;
        Time = DateTime.UnixEpoch.AddSeconds(time);
    }

    public long TargetUin { get; set; }

    public long SourceUin { get; set; }

    public State EventState { get; set; }

    public string Comment { get; set; }

    public string Source { get; set; }

    public DateTime Time { get; set; }

    public enum State
    {
        Pending = 1,
        Disapproved = 2,
        Approved = 3
    }
}
