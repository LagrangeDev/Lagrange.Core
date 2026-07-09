using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchGroupExtraEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x88d_0")]
internal class FetchGroupExtraService : OidbService<FetchGroupExtraEventReq, FetchGroupExtraEventResp, FetchGroupExtraRequest, FetchGroupExtraResponse>
{
    protected override uint Command => 0x88d;

    protected override uint Service => 0;

    protected override Task<FetchGroupExtraRequest> ProcessRequest(FetchGroupExtraEventReq request, BotContext context)
        => Task.FromResult(new FetchGroupExtraRequest
        {
            Random = Random.Shared.NextInt64(),
            Config = new FetchGroupExtraRequestConfig
            {
                GroupUin = request.GroupUin,
                Flags = new FetchGroupExtraRequestConfigFlags
                {
                    LatestMessageSequence = true,
                }
            }
        });

    protected override Task<FetchGroupExtraEventResp> ProcessResponse(FetchGroupExtraResponse response, BotContext context)
        => Task.FromResult(new FetchGroupExtraEventResp(new BotGroupExtra
        {
            LatestMessageSequence = response.Info.Result.LatestMessageSequence
        }));
}