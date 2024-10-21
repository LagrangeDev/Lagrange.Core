namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetTodoEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public uint Sequence { get; set; }

    public string? ResultMessage { get; set; }

    private GroupSetTodoEvent(uint groupUin, uint sequence) : base(0)
    {
        GroupUin = groupUin;
        Sequence = sequence;
    }

    private GroupSetTodoEvent(int resultCode, string? message) : base(resultCode) {
        ResultMessage = message;
    }

    public static GroupSetTodoEvent Create(uint groupUin, uint sequence) => new(groupUin, sequence);

    public static GroupSetTodoEvent Result(int resultCode, string? message) => new(resultCode, message);
}