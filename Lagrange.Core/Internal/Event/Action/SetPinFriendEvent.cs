namespace Lagrange.Core.Internal.Event.Action;

internal class SetPinFriendEvent : ProtocolEvent
{
    internal string Uid { get; set; }

    public uint Uin { get; set; }

    public bool IsPin { get; set; }

    public string Message { get; set; }

    protected SetPinFriendEvent(uint uin, bool isPin) : base(true)
    {
        Uid = string.Empty;
        Uin = uin;
        Message = string.Empty;
        IsPin = isPin;
    }

    protected SetPinFriendEvent(int retcode, string message) : base(retcode)
    {
        Uid = string.Empty;
        Uin = 0;
        Message = message;
    }

    public static SetPinFriendEvent Create(uint uin, bool isPin) => new(uin, isPin);

    public static SetPinFriendEvent Result(int retcode, string message) => new(retcode, message);
}