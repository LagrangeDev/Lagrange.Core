using Lagrange.Core.Message;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Event.Message;

internal class SendMessageEvent : ProtocolEvent
{
    public readonly MessageChain Chain;

    public readonly MessageResult MsgResult;
    
    protected SendMessageEvent(MessageChain chain) : base(true)
    {
        Chain = chain;
    }

    protected SendMessageEvent(int resultCode, MessageResult msgResult) : base(resultCode)
    {
        MsgResult = msgResult;
    }
    
    public static SendMessageEvent Create(MessageChain chain) => new(chain);
    
    public static SendMessageEvent Result(int resultCode, MessageResult msgResult) => new(resultCode, msgResult);
}