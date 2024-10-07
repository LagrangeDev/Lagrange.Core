namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFriendRequest
{
    public BotFriendRequest(string targetUid, string sourceUid, uint eventState, string comment, string source, uint time)
    {
        TargetUid = targetUid;
        SourceUid = sourceUid;
        EventState = (State)eventState;
        Comment = comment;
        Source = source;
        Time = DateTime.UnixEpoch.AddSeconds(time);
    }
    
    public string TargetUid { get; set; }

    public uint TargetUin { get; set; }
    
    public string SourceUid { get; set; }
    
    public uint SourceUin { get; set; }
    
    public State EventState { get; set; }
    
    
    public string Comment { get; set; }
    
    public string Source { get; set; }
    
    public DateTime Time { get; set; }
    
    public enum State
    {
        Pending = 1,
        Disapproved = 2,
        Approved = 3,
    }
}