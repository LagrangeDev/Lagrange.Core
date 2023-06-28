namespace Lagrange.Core.Core.Event.Protocol.Message;

internal class SendMessageEvent : ProtocolEvent
{
    protected SendMessageEvent(bool waitResponse) : base(true)
    {
    }

    protected SendMessageEvent(int resultCode) : base(resultCode)
    {
    }
}