namespace Lagrange.Core.Internal.Event.Action;

internal class GroupGetTodoEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public uint Sequence { get; set; }

    public string Preview { get; set; }

    public string? ResultMessage { get; set; }

    private GroupGetTodoEvent(uint groupUin) : base(0) {
        GroupUin = groupUin;
        Preview = string.Empty;
    }

    private GroupGetTodoEvent(int resultCode, string? message, uint groupUin, uint sequence, string preview) : base(resultCode)
    {
        ResultMessage = message;
        
        GroupUin = groupUin;
        Sequence = sequence;
        Preview = preview;
    }

    private GroupGetTodoEvent(int resultCode, string? message) : base(resultCode)
    {
        ResultMessage = message;
        Preview = string.Empty;
    }

    public static GroupGetTodoEvent Create(uint groupUin) => new(groupUin);

    public static GroupGetTodoEvent Result(int resultCode, string? message) => Result(resultCode, message, 0, 0, string.Empty);

    public static GroupGetTodoEvent Result(int resultCode, string? message, uint groupUin, uint sequence, string preview) => new(resultCode, message, groupUin, sequence, preview);
}