namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFinishTodoEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public string? ResultMessage { get; set; }

    private GroupFinishTodoEvent(uint groupUin) : base(0)
    {
        GroupUin = groupUin;
    }

    private GroupFinishTodoEvent(int resultCode, string? message) : base(resultCode) {
        ResultMessage = message;
    }

    public static GroupFinishTodoEvent Create(uint groupUin) => new(groupUin);

    public static GroupFinishTodoEvent Result(int resultCode, string? message) => new(resultCode, message);
}