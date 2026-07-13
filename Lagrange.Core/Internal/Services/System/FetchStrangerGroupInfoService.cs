using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchStrangerGroupInfoEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x88d_110")]
internal class FetchStrangerGroupInfoService : OidbService<FetchStrangerGroupInfoEventReq, FetchStrangerGroupInfoEventResp, D88DReqBody, D88DRspBody>
{
    protected override uint Command => 0x88d;

    protected override uint Service => 110;

    protected override Task<D88DReqBody> ProcessRequest(FetchStrangerGroupInfoEventReq request, BotContext context)
    {
        return Task.FromResult(new D88DReqBody
        {
            AppId = (uint)Random.Shared.Next(),
            Groups =
            [
                new D88DReqGroupInfo
                {
                    GroupCode = request.GroupUin,
                    GroupInfo = new D88DGroupInfo
                    {
                        GroupCreateTime = 0,
                        GroupMemberMaxNum = 0,
                        GroupMemberNum = 0,
                        GroupName = string.Empty,
                        GroupUin = 0
                    }
                }
            ]
        });
    }

    protected override Task<FetchStrangerGroupInfoEventResp> ProcessResponse(D88DRspBody response, BotContext context)
    {
        var group = response.Groups?.FirstOrDefault() ?? throw new OperationException(-1, response.ErrorInfo ?? "Group info was not returned");
        if (group.Result is not null and not 0) throw new OperationException((int)group.Result.Value, response.ErrorInfo);
        
        var info = group.GroupInfo ?? throw new OperationException(-1, response.ErrorInfo ?? "Group info was not returned");

        return Task.FromResult(new FetchStrangerGroupInfoEventResp(new BotStrangerGroupInfo
        {
            CreateTime = info.GroupCreateTime ?? 0,
            MaxMemberCount = info.GroupMemberMaxNum ?? 0,
            MemberCount = info.GroupMemberNum ?? 0,
            Name = info.GroupName ?? string.Empty,
            Uin = info.GroupUin ?? group.GroupCode ?? 0
        }));
    }
}
