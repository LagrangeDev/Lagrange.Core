using Lagrange.Core.Common;
using Lagrange.Core.Common.Response;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupGetTodoEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xf8e_1")]
internal class GroupGetTodoService : OidbService<GroupGetTodoEventReq, GroupGetTodoEventResp, DF8EReqBody, DF8ERspBody>
{
    protected override uint Command => 0xf8e;

    protected override uint Service => 1;

    protected override Task<DF8EReqBody> ProcessRequest(GroupGetTodoEventReq request, BotContext context)
    {
        return Task.FromResult(new DF8EReqBody
        {
            GroupCode = (ulong)request.GroupUin
        });
    }

    protected override Task<GroupGetTodoEventResp> ProcessResponse(DF8ERspBody response, BotContext context)
    {
        var info = response.Info ?? throw new OperationException(-1, "Group todo information was not returned");
        var result = new BotGetGroupTodoResult(info.GroupCode, info.Sequence, info.Title ?? string.Empty);
        return Task.FromResult(new GroupGetTodoEventResp(result));
    }
}
