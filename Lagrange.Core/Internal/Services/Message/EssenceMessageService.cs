using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<SetEssenceMessageEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xeac_1")]
internal class SetEssenceMessageService : OidbService<SetEssenceMessageEventReq, SetEssenceMessageEventResp, EACReqBody, EACRspBody>
{
    protected override uint Command => 0xeac;

    protected override uint Service => 1;

    protected override Task<EACReqBody> ProcessRequest(SetEssenceMessageEventReq request, BotContext context)
    {
        return Task.FromResult(new EACReqBody
        {
            GroupUin = (ulong)request.GroupUin,
            Sequence = request.Sequence,
            Random = request.Random
        });
    }

    protected override Task<SetEssenceMessageEventResp> ProcessResponse(EACRspBody response, BotContext context)
    {
        return Task.FromResult(SetEssenceMessageEventResp.Default);
    }
}

[EventSubscribe<RemoveEssenceMessageEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xeac_2")]
internal class RemoveEssenceMessageService : OidbService<RemoveEssenceMessageEventReq, RemoveEssenceMessageEventResp, EACReqBody, EACRspBody>
{
    protected override uint Command => 0xeac;

    protected override uint Service => 2;

    protected override Task<EACReqBody> ProcessRequest(RemoveEssenceMessageEventReq request, BotContext context)
    {
        return Task.FromResult(new EACReqBody
        {
            GroupUin = (ulong)request.GroupUin,
            Sequence = request.Sequence,
            Random = request.Random
        });
    }

    protected override Task<RemoveEssenceMessageEventResp> ProcessResponse(EACRspBody response, BotContext context)
    {
        return Task.FromResult(RemoveEssenceMessageEventResp.Default);
    }
}
