namespace Lagrange.Core.Internal.Event.Action;

internal class SetGroupRequestEvent : ProtocolEvent
{
    public ulong Sequence { get; set; }

    public uint GroupUin { get; set; }
    
    public bool Accept { get; set; }
    
    public uint Type { get; }

    private SetGroupRequestEvent(bool accept, uint groupUin, ulong sequence, uint type) : base(true)
    {
        Accept = accept;
        GroupUin = groupUin;
        Sequence = sequence;
        Type = type;
    }
    
    private SetGroupRequestEvent(int resultCode) : base(resultCode) { }
    
    public static SetGroupRequestEvent Create(bool accept, uint groupUin, ulong sequence, uint type) 
        => new(accept, groupUin, sequence, type);
    
    public static SetGroupRequestEvent Result(int resultCode) => new(resultCode);
}