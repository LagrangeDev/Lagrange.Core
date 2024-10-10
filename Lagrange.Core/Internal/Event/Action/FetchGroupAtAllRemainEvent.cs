namespace Lagrange.Core.Internal.Event.Action;

internal class FetchGroupAtAllRemainEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public uint RemainAtAllCountForUin { get; set; }
    
    public uint RemainAtAllCountForGroup { get; set; }
    
    private FetchGroupAtAllRemainEvent(uint groupUin) : base(true)
    {
        GroupUin = groupUin;
    }

    private FetchGroupAtAllRemainEvent(int resultCode, uint remainAtAllCountForUin, uint remainAtAllCountForGroup) : base(resultCode)
    {
        RemainAtAllCountForUin = remainAtAllCountForUin;
        RemainAtAllCountForGroup = remainAtAllCountForGroup;
    }
    
    public static FetchGroupAtAllRemainEvent Create(uint groupUin) => new(groupUin);
    
    public static FetchGroupAtAllRemainEvent Result(int resultCode, uint remainAtAllCountForUin, uint remainAtAllCountForGroup) =>
        new(resultCode, remainAtAllCountForUin, remainAtAllCountForGroup);
}