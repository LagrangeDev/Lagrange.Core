using Lagrange.Core.Message;

namespace Lagrange.Core.Event.EventArg;

public class FriendMessageEvent : EventBase
{
    public MessageChain Chain { get; set; }
    
    public FriendMessageEvent(MessageChain chain)
    {
        Chain = chain;
    }
}