namespace Lagrange.Core.Internal.Event.Message;

internal class RecallGroupMessageEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public ulong Sequence { get; set; }
    
    private RecallGroupMessageEvent(ulong sequence, uint groupUin) : base(true)
    {
        Sequence = sequence;
        GroupUin = groupUin;
    }
    
    private RecallGroupMessageEvent(int resultCode) : base(resultCode) { }
    
    public static RecallGroupMessageEvent Create(uint groupUin, ulong sequence) =>
            new(sequence, groupUin);
    
    public static RecallGroupMessageEvent Result(int resultCode) => new(resultCode);
}