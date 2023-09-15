using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Event.EventArg;

public class FriendMessageEvent : EventBase
{
    public MessageChain Chain { get; set; }
    
    public FriendMessageEvent(MessageChain chain)
    {
        Chain = chain;
    }
}