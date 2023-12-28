namespace Lagrange.Core.Internal.Event.Action;

internal class AcceptGroupRequestEvent : ProtocolEvent
{
    public ulong Sequence { get; set; }
    
    public uint GroupUin { get; set; }
    
    public bool Accept { get; set; }

    private AcceptGroupRequestEvent(bool accept, uint groupUin, ulong sequence) : base(true)
    {
        Accept = accept;
        GroupUin = groupUin;
        Sequence = sequence;
    }
    
    private AcceptGroupRequestEvent(int resultCode) : base(resultCode) { }
    
    public static AcceptGroupRequestEvent Create(bool accept, uint groupUin, ulong sequence) => new(accept, groupUin, sequence);
    
    public static AcceptGroupRequestEvent Result(int resultCode) => new(resultCode);
}