namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysPokeEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public uint OperatorUin { get; }
    
    public uint TargetUin { get; }

    public string Action { get; }
    
    public string Suffix { get; }
    
    private GroupSysPokeEvent(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix) : base(0)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
    }
    
    public static GroupSysPokeEvent Result(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix) 
        => new(groupUin, operatorUin, targetUin, action, suffix);
}