using Lagrange.Core.Message;

namespace Lagrange.Core.Event.EventArg;

public class GroupProMessageEvent : EventBase
{
    public MessageChain Chain { get; set; }
    
    public GroupProMessageEvent(MessageChain chain)
    {
        Chain = chain;
    }
}