namespace Lagrange.Core.Internal.Event.Action;

internal class SetPinGroupEvent : ProtocolEvent
{
    public uint Uin { get; set; }

    public bool IsPin { get; set; }

    public string Message { get; set; }

    protected SetPinGroupEvent(uint uin, bool isPin) : base(true)
    {
        Uin = uin;
        Message = string.Empty;
        IsPin = isPin;
    }

    protected SetPinGroupEvent(int retcode, string message) : base(retcode)
    {
        Uin = 0;
        Message = message;
    }

    public static SetPinGroupEvent Create(uint uin, bool isPin) => new(uin, isPin);

    public static SetPinGroupEvent Result(int retcode, string message) => new(retcode, message);
}