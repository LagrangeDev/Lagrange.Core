using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupRemarkEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xf16_1")]
internal class GroupRemarkService : OidbService<GroupRemarkEventReq, GroupRemarkEventResp, DF16ReqBody, DF16RspBody>
{
    protected override uint Command => 0xf16;

    protected override uint Service => 1;

    protected override Task<DF16ReqBody> ProcessRequest(GroupRemarkEventReq request, BotContext context)
    {
        return Task.FromResult(new DF16ReqBody
        {
            RemarkInfo = new F16GroupRemarkInfoReq
            {
                GroupCode = (ulong)request.GroupUin,
                RemarkName = Encoding.UTF8.GetBytes(request.TargetRemark)
            }
        });
    }

    protected override Task<GroupRemarkEventResp> ProcessResponse(DF16RspBody response, BotContext context)
    {
        return Task.FromResult(GroupRemarkEventResp.Default);
    }
}
