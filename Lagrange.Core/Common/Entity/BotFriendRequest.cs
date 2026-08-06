namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFriendRequest(long targetUin, long sourceUin, uint eventState, string comment, string source, uint time)
{
    public long TargetUin { get; set; } = targetUin;

    public long SourceUin { get; set; } = sourceUin;

    public State EventState { get; set; } = (State)eventState;

    public string Comment { get; set; } = comment;

    public string Source { get; set; } = source;

    public long Time { get; set; } = time;

    public enum State
    {
        Pending = 1,
        Disapproved = 2,
        Approved = 3
    }
}
