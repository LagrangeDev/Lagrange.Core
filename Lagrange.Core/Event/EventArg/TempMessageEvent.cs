using Lagrange.Core.Message;

namespace Lagrange.Core.Event.EventArg;

public class TempMessageEvent : EventBase
{
    public MessageChain Chain { get; set; }
    
    public TempMessageEvent(MessageChain chain)
    {
        Chain = chain;
    }
}