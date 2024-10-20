namespace Lagrange.Core.Internal.Event.Action;

internal class GroupRemoveTodoEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public string? ResultMessage { get; set; }

    private GroupRemoveTodoEvent(uint groupUin) : base(0)
    {
        GroupUin = groupUin;
    }

    private GroupRemoveTodoEvent(int resultCode, string? message) : base(resultCode) {
        ResultMessage = message;
    }

    public static GroupRemoveTodoEvent Create(uint groupUin) => new(groupUin);

    public static GroupRemoveTodoEvent Result(int resultCode, string? message) => new(resultCode, message);
}