using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;
using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Events.Message;

internal class LongMsgSendEventReq(BotContact receiver, List<BotMessage> messages) : ProtocolEvent
{
    public BotContact Receiver { get; } = receiver;
    
    public List<BotMessage> Messages { get; } = messages;
}

internal class LongMsgSendEventResp(string resId) : ProtocolEvent
{
    public string ResId { get; } = resId;
}