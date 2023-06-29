namespace Lagrange.Core.Core.Event.Protocol.Message;

internal class PushMessageEvent : ProtocolEvent
{
    private PushMessageEvent() : base(false)
    {
    }

    private PushMessageEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static PushMessageEvent Create() => new(0);
    
    public enum MessageType
    {
        Friend,
        Group,
        Temp,
    }
}