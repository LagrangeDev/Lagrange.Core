using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupSetSpecialTitleEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x8fc_2")]
internal class GroupSetSpecialTitleService : OidbService<GroupSetSpecialTitleEventReq, GroupSetSpecialTitleEventResp, D8FCReqBody, D8FCRspBody>
{
    private protected override uint Command => 0x8fc;

    private protected override uint Service => 2;

    private protected override Task<D8FCReqBody> ProcessRequest(GroupSetSpecialTitleEventReq request, BotContext context)
    {
        return Task.FromResult(new D8FCReqBody
        {
            GroupCode = request.GroupUin,
            MemLevelInfo = [
                new()
                {
                    Uid = request.TargetUid,
                    SpecialTitle = Encoding.UTF8.GetBytes(request.Title),
                }
            ]
        });
    }

    private protected override Task<GroupSetSpecialTitleEventResp> ProcessResponse(D8FCRspBody response, BotContext context)
    {
        return Task.FromResult(GroupSetSpecialTitleEventResp.Default);
    }
}
