namespace Lagrange.Core.Internal.Event.Notify;

internal class SysPinChangedEvent : ProtocolEvent
{
    public string Uid { get; }

    public uint? GroupUin { get; }

    public bool IsPin { get; }

    private SysPinChangedEvent(string uid, uint? groupUin, bool isPin) : base(0)
    {
        Uid = uid;
        GroupUin = groupUin;
        IsPin = isPin;
    }

    public static SysPinChangedEvent Result(string uid, uint? groupUin, bool isPin) => new(uid, groupUin, isPin);
}