using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupSetTodoEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xf90_1")]
internal class GroupSetTodoService : OidbService<GroupSetTodoEventReq, GroupSetTodoEventResp, F90ReqBody, F90RspBody>
{
    private protected override uint Command => 0xf90;

    private protected override uint Service => 1;

    private protected override Task<F90ReqBody> ProcessRequest(GroupSetTodoEventReq request, BotContext context)
    {
        return Task.FromResult(new F90ReqBody
        {
            GroupCode = (ulong)request.GroupUin,
            Seq = request.Sequence
        });
    }

    private protected override Task<GroupSetTodoEventResp> ProcessResponse(F90RspBody response, BotContext context)
    {
        return Task.FromResult(GroupSetTodoEventResp.Default);
    }
}

[EventSubscribe<GroupFinishTodoEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xf90_2")]
internal class GroupFinishTodoService : OidbService<GroupFinishTodoEventReq, GroupFinishTodoEventResp, F90ReqBody, F90RspBody>
{
    private protected override uint Command => 0xf90;

    private protected override uint Service => 2;

    private protected override Task<F90ReqBody> ProcessRequest(GroupFinishTodoEventReq request, BotContext context)
    {
        return Task.FromResult(new F90ReqBody
        {
            GroupCode = (ulong)request.GroupUin
        });
    }

    private protected override Task<GroupFinishTodoEventResp> ProcessResponse(F90RspBody response, BotContext context)
    {
        return Task.FromResult(GroupFinishTodoEventResp.Default);
    }
}

[EventSubscribe<GroupRemoveTodoEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xf90_3")]
internal class GroupRemoveTodoService : OidbService<GroupRemoveTodoEventReq, GroupRemoveTodoEventResp, F90ReqBody, F90RspBody>
{
    private protected override uint Command => 0xf90;

    private protected override uint Service => 3;

    private protected override Task<F90ReqBody> ProcessRequest(GroupRemoveTodoEventReq request, BotContext context)
    {
        return Task.FromResult(new F90ReqBody
        {
            GroupCode = (ulong)request.GroupUin
        });
    }

    private protected override Task<GroupRemoveTodoEventResp> ProcessResponse(F90RspBody response, BotContext context)
    {
        return Task.FromResult(GroupRemoveTodoEventResp.Default);
    }
}
