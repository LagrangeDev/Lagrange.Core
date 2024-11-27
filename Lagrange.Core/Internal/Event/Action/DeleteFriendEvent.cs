namespace Lagrange.Core.Internal.Event.Action;

internal class DeleteFriendEvent : ProtocolEvent
{
    public string? TargetUid { get; set; }

    public bool Block { get; set; }

    private DeleteFriendEvent(string? targetUid, bool block) : base(true)
    {
        TargetUid = targetUid;
        Block = block;
    }

    private DeleteFriendEvent(int resultCode) : base(resultCode) { }

    public static DeleteFriendEvent Create(string? targetUid, bool block) => new (targetUid, block);

    public static DeleteFriendEvent Result(int resultCode) => new (resultCode);
}