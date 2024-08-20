namespace Lagrange.Core.Event.EventArg;

public class FriendPokeEvent : EventBase
{
    public uint OperatorUin { get; }
    
    public uint TargetUin { get; }
    
    public string Action { get; }
    
    public string Suffix { get; }

    public string ActionImgUrl { get; }
    
    public FriendPokeEvent(uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl)
    { 
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImgUrl = actionImgUrl;
        
        EventMessage = $"{nameof(FriendPokeEvent)}: OperatorUin: {OperatorUin} | TargetUin: {TargetUin} | Action: {Action} | Suffix: {Suffix} | ActionImgUrl: {ActionImgUrl}";
    }
}
