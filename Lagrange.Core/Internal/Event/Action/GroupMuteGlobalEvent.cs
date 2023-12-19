namespace Lagrange.Core.Internal.Event.Action;

internal class GroupMuteGlobalEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public bool IsMute { get; set; }

    private GroupMuteGlobalEvent(uint groupUin, bool isMute) : base(true)
    {
        GroupUin = groupUin;
        IsMute = isMute;
    }

    private GroupMuteGlobalEvent(int resultCode) : base(resultCode) { }
    
    public static GroupMuteGlobalEvent Create(uint groupUin, bool isMute) => new(groupUin, isMute);
    
    public static GroupMuteGlobalEvent Result(int resultCode) => new(resultCode);
}