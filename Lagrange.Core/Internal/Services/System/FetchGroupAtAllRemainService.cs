using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchGroupAtAllRemainEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x8a7_0")]
internal class FetchGroupAtAllRemainService : OidbService<FetchGroupAtAllRemainEventReq, FetchGroupAtAllRemainEventResp, D8A7ReqBody, D8A7RspBody>
{
    protected override uint Command => 0x8a7;

    protected override uint Service => 0;

    protected override Task<D8A7ReqBody> ProcessRequest(FetchGroupAtAllRemainEventReq request, BotContext context)
    {
        return Task.FromResult(new D8A7ReqBody
        {
            SubCommand = 1,
            LimitIntervalTypeForUin = 2,
            LimitIntervalTypeForGroup = 1,
            Uin = (ulong)context.BotUin,
            GroupCode = (ulong)request.GroupUin
        });
    }

    protected override Task<FetchGroupAtAllRemainEventResp> ProcessResponse(D8A7RspBody response, BotContext context) =>
        Task.FromResult(new FetchGroupAtAllRemainEventResp(response.CanAtAll, response.RemainAtAllCountForUin, response.RemainAtAllCountForGroup));
}
