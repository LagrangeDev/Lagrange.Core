using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupRenameEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x89a_15")]
internal class GroupRenameService : OidbService<GroupRenameEventReq, GroupRenameEventResp, D89AReqBody, D89ARspBody>
{
    protected override uint Command => 0x89a;

    protected override uint Service => 15;

    protected override Task<D89AReqBody> ProcessRequest(GroupRenameEventReq request, BotContext context)
    {
        return Task.FromResult(new D89AReqBody
        {
            GroupCode = request.GroupUin,
            Group = new()
            {
                GroupName = request.TargetName
            },
        });
    }

    protected override Task<GroupRenameEventResp> ProcessResponse(D89ARspBody response, BotContext context)
    {
        return Task.FromResult(GroupRenameEventResp.Default);
    }
}
