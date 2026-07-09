using Lagrange.Core.Events;
using Lagrange.Core.Message;

namespace Lagrange.Core.Internal.Events.Message;

internal class LongMsgRecvEventReq(bool isGroup, string resId) : ProtocolEvent
{
    public bool IsGroup { get; } = isGroup;

    public string ResId { get; } = resId;
}

internal class LongMsgRecvEventResp(List<BotMessage> messages) : ProtocolEvent
{
    public List<BotMessage> Messages { get; } = messages;
}