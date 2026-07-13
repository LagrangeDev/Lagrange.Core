using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchGroupsEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xfe5_2")]
internal class FetchGroupsService : OidbService<FetchGroupsEventReq, FetchGroupsEventResp, FetchGroupsRequest, FetchGroupsResponse>
{
    protected override uint Command => 0xfe5;

    protected override uint Service => 2;

    protected override Task<FetchGroupsRequest> ProcessRequest(FetchGroupsEventReq request, BotContext context)
    {
        return Task.FromResult(new FetchGroupsRequest
        {
            Config = new FetchGroupsRequestConfig
            {
                Config1 = new FetchGroupsRequestConfig1 { },
                Config2 = new FetchGroupsRequestConfig2 { },
                Config3 = new FetchGroupsRequestConfig3 { },
            },
        });
    }

    protected override Task<FetchGroupsEventResp> ProcessResponse(FetchGroupsResponse response, BotContext context)
    {
        return Task.FromResult(new FetchGroupsEventResp([.. response.Groups
            .Select(raw => new BotGroup(
                raw.GroupUin,
                raw.Info.GroupName,
                (int)raw.Info.MemberCount,
                (int)raw.Info.MemberMax,
                raw.Info.CreatedTime,
                raw.Info.Description,
                raw.Info.Question,
                raw.Info.Announcement,
                raw.CustomInfo.Remark
            ))]));
    }
}
