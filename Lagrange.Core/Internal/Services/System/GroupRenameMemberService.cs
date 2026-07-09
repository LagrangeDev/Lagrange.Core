using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupMemberRenameEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x8fc_3")]
internal class GroupRenameMemberService: OidbService<GroupMemberRenameEventReq, GroupMemberRenameEventResp, D8FCReqBody, D8FCRspBody>
{
    protected override uint Command => 0x8fc;

    protected override uint Service => 3;

    protected override Task<D8FCReqBody> ProcessRequest(GroupMemberRenameEventReq request, BotContext context)
    {
        return Task.FromResult(new D8FCReqBody
        {
            GroupCode = request.GroupUin,
            MemLevelInfo = [
                new()
                {
                    Uid = request.TargetUid,
                    MemberCardName = Encoding.UTF8.GetBytes(request.Name),
                }
            ]
        });
    }

    protected override Task<GroupMemberRenameEventResp> ProcessResponse(D8FCRspBody response, BotContext context)
    {
        return Task.FromResult(GroupMemberRenameEventResp.Default);
    }
}