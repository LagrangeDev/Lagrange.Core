namespace Lagrange.Core.Event.EventArg;

public class GroupMuteEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint? OperatorUin { get; }
    
    public bool IsMuted { get; }
    
    public GroupMuteEvent(uint groupUin, uint? operatorUin, bool isMuted)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        IsMuted = isMuted;
        
        EventMessage = $"{nameof(GroupMuteEvent)}: {GroupUin} | {OperatorUin} | IsMuted: {IsMuted}";
    }
}