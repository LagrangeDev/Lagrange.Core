using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchClientKeyEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x102a_1")]
internal class FetchClientKeyService : OidbService<FetchClientKeyEventReq, FetchClientKeyEventResp, D102AReqBody, D102ARspBody>
{
    protected override uint Command => 0x102A;
    
    protected override uint Service => 1;
    
    protected override Task<D102AReqBody> ProcessRequest(FetchClientKeyEventReq request, BotContext context)
    {
        return Task.FromResult(new D102AReqBody());
    }

    protected override Task<FetchClientKeyEventResp> ProcessResponse(D102ARspBody response, BotContext context)
    {
        return Task.FromResult(new FetchClientKeyEventResp(response.ClientKey, response.Expiration));
    }
}