namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysPokeEvent : ProtocolEvent
{
    public uint OperatorUin { get; }
    
    public uint TargetUin { get; }

    public string Action { get; }
    
    public string Suffix { get; }
    
    public string ActionImgUrl { get; }
    
    private FriendSysPokeEvent(uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl) : base(0)
    {
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImgUrl = actionImgUrl;
    }
    
    public static FriendSysPokeEvent Result(uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl) 
        => new(operatorUin, targetUin, action, suffix, actionImgUrl);
}
