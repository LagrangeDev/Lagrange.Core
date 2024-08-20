namespace Lagrange.Core.Event.EventArg;

public class GroupPokeEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint OperatorUin { get; }
    
    public uint TargetUin { get; }
    
    public string Action { get; }
    
    public string Suffix { get; }
    
    public string ActionImgUrl { get; }
    
    public GroupPokeEvent(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImgUrl = actionImgUrl;
        
        EventMessage = $"{nameof(GroupPokeEvent)}:  GroupUin: {GroupUin} | OperatorUin: {OperatorUin} | TargetUin: {TargetUin} | Action: {Action} | Suffix: {Suffix} | ActionImgUrl: {ActionImgUrl}";
    }
}
