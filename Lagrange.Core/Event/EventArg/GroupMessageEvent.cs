using Lagrange.Core.Message;

namespace Lagrange.Core.Event.EventArg;

public class GroupMessageEvent : EventBase
{
    public MessageChain Chain { get; set; }
    
    public GroupMessageEvent(MessageChain chain)
    {
        Chain = chain;
    }
}