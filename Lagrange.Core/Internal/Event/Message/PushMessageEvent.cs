using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.Message;

internal class PushMessageEvent : ProtocolEvent
{
    public MessageChain Chain { get; set; }

    private PushMessageEvent(int resultCode, MessageChain chain) : base(resultCode)
    {
        Chain = chain;
    }
    
    public static PushMessageEvent Create(MessageChain chain) => new(0, chain);
}