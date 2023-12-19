namespace Lagrange.Core.Internal.Event.Action;

internal class RequestFriendEvent : ProtocolEvent
{
    public uint TargetUin { get; set; }
    
    public string Question { get; set; } = "";
    
    public string Message { get; set; } = "";

    protected RequestFriendEvent(uint targetUin, string question, string message) : base(true)
    {
        TargetUin = targetUin;
        Question = question;
        Message = message;
    }
    
    protected RequestFriendEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static RequestFriendEvent Create(uint targetUin, string question, string message) => new(targetUin, question, message);

    public static RequestFriendEvent Result(int resultCode) => new(resultCode);
}