using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupMuteMemberEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x1253_1")]
internal class GroupMuteMemberService : OidbService<GroupMuteMemberEventReq, GroupMuteMemberEventResp, D1253ReqBody, D1253RspBody>
{
    protected override uint Command => 0x1253;

    protected override uint Service => 1;

    protected override Task<D1253ReqBody> ProcessRequest(GroupMuteMemberEventReq request, BotContext context)
    {
        return Task.FromResult(new D1253ReqBody
        {
            GroupUin = request.GroupUin,
            Type = 1,
            Body = new D1253ReqBody.MuteInfo
            {
                TargetUid = request.TargetUid,
                Duration = request.Duration
            }
        });
    }

    protected override Task<GroupMuteMemberEventResp> ProcessResponse(D1253RspBody response, BotContext context) =>
        Task.FromResult(GroupMuteMemberEventResp.Default);
}
