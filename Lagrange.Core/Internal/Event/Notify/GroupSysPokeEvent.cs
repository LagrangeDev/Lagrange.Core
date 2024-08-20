namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysPokeEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public uint OperatorUin { get; }
    
    public uint TargetUin { get; }

    public string Action { get; }
    
    public string Suffix { get; }
    
    public string ActionImgUrl { get; }
    
    private GroupSysPokeEvent(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl) : base(0)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        TargetUin = targetUin;
        Action = action;
        Suffix = suffix;
        ActionImgUrl = actionImgUrl;
    }
    
    public static GroupSysPokeEvent Result(uint groupUin, uint operatorUin, uint targetUin, string action, string suffix, string actionImgUrl) 
        => new(groupUin, operatorUin, targetUin, action, suffix, actionImgUrl);
}
