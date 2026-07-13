using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupMuteGlobalEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x89a_0")]
internal class GroupMuteGlobalService : OidbService<GroupMuteGlobalEventReq, GroupMuteGlobalEventResp, D89AReqBody, D89ARspBody>
{
    protected override uint Command => 0x89a;

    protected override uint Service => 0;

    protected override Task<D89AReqBody> ProcessRequest(GroupMuteGlobalEventReq request, BotContext context)
    {
        return Task.FromResult(new D89AReqBody
        {
            GroupCode = request.GroupUin,
            Group = new D89AReqBody.GroupInfo
            {
                ShutupTime = request.IsMute ? uint.MaxValue : 0
            }
        });
    }

    protected override Task<GroupMuteGlobalEventResp> ProcessResponse(D89ARspBody response, BotContext context) =>
        Task.FromResult(GroupMuteGlobalEventResp.Default);
}
