namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysMuteEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string? OperatorUid { get; }
    
    public bool IsMuted { get; }
    
    private GroupSysMuteEvent(uint groupUin, string? operatorUid, bool isMuted) : base(0)
    {
        GroupUin = groupUin;
        OperatorUid = operatorUid;
        IsMuted = isMuted;
    }
    
    public static GroupSysMuteEvent Result(uint groupUin, string? operatorUid, bool isMuted) =>
        new(groupUin, operatorUid, isMuted);
}