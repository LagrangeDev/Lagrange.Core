namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetReactionEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public uint Sequence { get; }

    public string Code { get; } = string.Empty;

    private GroupSetReactionEvent(uint groupUin, uint sequence, string code) : base(true)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Code = code;
    }

    private GroupSetReactionEvent(int resultCode) : base(resultCode) { }

    public static GroupSetReactionEvent Create(uint groupUin, uint sequence, string code)
        => new(groupUin, sequence, code);

    public static GroupSetReactionEvent Result(int resultCode) => new(resultCode);
}