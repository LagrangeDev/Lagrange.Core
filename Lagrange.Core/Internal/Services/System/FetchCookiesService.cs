using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchCookiesEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x102a_0")]
internal class FetchCookiesService : OidbService<FetchCookiesEventReq, FetchCookiesEventResp, D102AReqBody, D102ARspBody>
{
    protected override uint Command => 0x102A;

    protected override uint Service => 0;
    
    protected override Task<D102AReqBody> ProcessRequest(FetchCookiesEventReq request, BotContext context)
    {
        return Task.FromResult(new D102AReqBody { Domain = request.Domain });
    }

    protected override Task<FetchCookiesEventResp> ProcessResponse(D102ARspBody response, BotContext context)
    {
        return Task.FromResult(new FetchCookiesEventResp(response.PsKeys));
    }
}