namespace Lagrange.Core.Internal.Event.Message;

internal class PokeEvent : ProtocolEvent
{
    public uint PeerUin { get; }
    public uint? TargetUin { get; }
    public bool IsGroup { get; }

    protected PokeEvent(bool isGroup, uint peerUin, uint? targetUin) : base(true)
    {
        PeerUin = peerUin;
        TargetUin = targetUin;
        IsGroup = isGroup;
    }

    protected PokeEvent(int resultCode) : base(resultCode) { }
    public static PokeEvent Create(bool isGroup, uint peerUin, uint? targetUin) => new(isGroup, peerUin, targetUin);

    public static PokeEvent Result(int resultCode) => new(resultCode);
}