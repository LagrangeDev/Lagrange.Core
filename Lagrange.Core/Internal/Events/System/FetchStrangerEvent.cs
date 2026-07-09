using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal abstract class FetchStrangerEventReqBase : ProtocolEvent { }

internal class FetchStrangerByUinEventReq(long uin) : FetchStrangerEventReqBase
{
    public long Uin { get; } = uin;
}

internal class FetchStrangerByUidEventReq(string uid) : FetchStrangerEventReqBase
{
    public string Uid { get; } = uid;
}

internal class FetchStrangerEventResp(BotStranger stranger) : ProtocolEvent
{
    public BotStranger Stranger { get; } = stranger;
}