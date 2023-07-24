using Lagrange.Core.Message;

#pragma warning disable CS8618
namespace Lagrange.Core.Core.Event.Protocol.Message;

internal class SendMessageEvent : ProtocolEvent
{
    public MessageChain Chain;
    
    protected SendMessageEvent(MessageChain chain) : base(true)
    {
        Chain = chain;
    }

    protected SendMessageEvent(int resultCode) : base(resultCode)
    {
    }
    
    public static SendMessageEvent Create(MessageChain chain) => new(chain);
    
    public static SendMessageEvent Result(int resultCode) => new(resultCode);
}