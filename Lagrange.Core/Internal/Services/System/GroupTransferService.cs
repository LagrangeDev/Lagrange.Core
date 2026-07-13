using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupTransferEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x89e_0")]
internal class GroupTransferService : OidbService<GroupTransferEventReq, GroupTransferEventResp, D89EReqBody, D89ERspBody>
{
    protected override uint Command => 0x89e;

    protected override uint Service => 0;

    protected override Task<D89EReqBody> ProcessRequest(GroupTransferEventReq request, BotContext context)
    {
        return Task.FromResult(new D89EReqBody
        {
            GroupCode = checked((ulong)request.GroupUin),
            OldOwner = checked((ulong)context.BotUin),
            NewOwner = checked((ulong)request.TargetUin)
        });
    }

    protected override Task<GroupTransferEventResp> ProcessResponse(D89ERspBody response, BotContext context)
    {
        return Task.FromResult(GroupTransferEventResp.Default);
    }
}
