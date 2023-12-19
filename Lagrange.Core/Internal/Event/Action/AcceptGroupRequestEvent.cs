namespace Lagrange.Core.Internal.Event.Action;

internal class AcceptGroupRequestEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public bool Accept { get; set; }

    protected AcceptGroupRequestEvent(bool accept, uint groupUin) : base(true)
    {
        Accept = accept;
        GroupUin = groupUin;
    }
    
    protected AcceptGroupRequestEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static AcceptGroupRequestEvent Create(bool accept, uint groupUin) => new(accept, groupUin);
    
    public static AcceptGroupRequestEvent Result(int resultCode) => new(resultCode);
}