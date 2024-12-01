namespace Lagrange.Core.Internal.Event.Action;

internal class GroupReduceReactionEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint Sequence { get; }

    public string Code { get; } = string.Empty;

    public bool IsEmoji => Code.Length > 3;

    private GroupReduceReactionEvent(uint groupUin, uint sequence, string code) : base(true)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Code = code;
    }

    private GroupReduceReactionEvent(int resultCode) : base(resultCode) { }

    public static GroupReduceReactionEvent Create(uint groupUin, uint sequence, string code)
        => new(groupUin, sequence, code);

    public static GroupReduceReactionEvent Result(int resultCode) => new(resultCode);
}