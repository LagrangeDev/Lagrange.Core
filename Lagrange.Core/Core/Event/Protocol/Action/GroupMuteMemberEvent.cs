namespace Lagrange.Core.Core.Event.Protocol.Action;

internal class GroupMuteMemberEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public int Duration { get; set; }
    
    public string Uid { get; set; } = "";

    private GroupMuteMemberEvent(uint groupUin, int duration, string uid) : base(0)
    {
        GroupUin = groupUin;
        Duration = duration;
        Uid = uid;
    }

    private GroupMuteMemberEvent(int resultCode) : base(resultCode) { }
    
    public static GroupMuteMemberEvent Create(uint groupUin, int duration, string uid) => new(groupUin, duration, uid);

    public static GroupMuteMemberEvent Result(int resultCode) => new(resultCode);
}