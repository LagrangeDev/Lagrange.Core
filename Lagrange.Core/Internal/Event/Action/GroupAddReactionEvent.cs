namespace Lagrange.Core.Internal.Event.Action;

internal class GroupAddReactionEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint Sequence { get; }

    public string Code { get; } = string.Empty;

    public bool IsEmoji => Code.Length > 3;

    private GroupAddReactionEvent(uint groupUin, uint sequence, string code) : base(true)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        Code = code;
    }

    private GroupAddReactionEvent(int resultCode) : base(resultCode) { }

    public static GroupAddReactionEvent Create(uint groupUin, uint sequence, string code)
        => new(groupUin, sequence, code);

    public static GroupAddReactionEvent Result(int resultCode) => new(resultCode);
}