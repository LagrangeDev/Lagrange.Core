namespace Lagrange.Core.Event.EventArg;

public class GroupPokeEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint OperatorUin { get; }
    
    public uint TargetUin { get; }
    
    public string Action { get; }
    
    public string Suffix { get; }
    
    public GroupPokeEvent(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        
        EventMessage = $"{nameof(GroupPokeEvent)}:  GroupUin: {GroupUin} | OperatorUin: {OperatorUin} | TargetUin: {TargetUin} | Action: {Action} | Suffix: {Suffix}";
    }
}