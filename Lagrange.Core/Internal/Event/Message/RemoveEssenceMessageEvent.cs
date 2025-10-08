namespace Lagrange.Core.Internal.Event.Message;

internal class RemoveEssenceMessageEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public ulong Sequence { get; }
    
    public uint Random { get; }

    private RemoveEssenceMessageEvent(uint groupUin, ulong sequence, uint random) : base(true)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Random = random;
    }

    private RemoveEssenceMessageEvent(int resultCode) : base(resultCode) { }

    public static RemoveEssenceMessageEvent Create(uint groupUin, ulong sequence, uint random)
        => new(groupUin, sequence, random);

    public static RemoveEssenceMessageEvent Result(int resultCode)
        => new(resultCode);
}