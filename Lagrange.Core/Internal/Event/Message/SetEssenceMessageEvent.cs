namespace Lagrange.Core.Internal.Event.Message;

internal class SetEssenceMessageEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public uint Sequence { get; }
    
    public uint Random { get; }

    private SetEssenceMessageEvent(uint groupUin, uint sequence, uint random) : base(true)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Random = random;
    }

    private SetEssenceMessageEvent(int resultCode) : base(resultCode) { }

    public static SetEssenceMessageEvent Create(uint groupUin, uint sequence, uint random)
        => new(groupUin, sequence, random);

    public static SetEssenceMessageEvent Result(int resultCode)
        => new(resultCode);
}