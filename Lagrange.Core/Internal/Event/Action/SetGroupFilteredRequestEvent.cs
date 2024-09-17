namespace Lagrange.Core.Internal.Event.Action;

internal class SetGroupFilteredRequestEvent : ProtocolEvent
{
    public ulong Sequence { get; set; }

    public uint GroupUin { get; set; }
    
    public bool Accept { get; set; }
    
    public uint Type { get; }
    
    public string Reason { get; set; } = string.Empty;

    private SetGroupFilteredRequestEvent(bool accept, uint groupUin, ulong sequence, uint type, string? reason) : base(true)
    {
        Accept = accept;
        GroupUin = groupUin;
        Sequence = sequence;
        Type = type;
        Reason = reason ?? "";
    }
    
    private SetGroupFilteredRequestEvent(int resultCode) : base(resultCode) { }

    public static SetGroupFilteredRequestEvent Create(bool accept, uint groupUin, ulong sequence, uint type, string? reason) 
        => new(accept, groupUin, sequence, type, reason);
    
    public static SetGroupFilteredRequestEvent Result(int resultCode) => new(resultCode);
}