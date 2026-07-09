using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchGroupMembersEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xfe7_3")]
internal class FetchGroupMembersService : OidbService<FetchGroupMembersEventReq, FetchGroupMembersEventResp, FetchGroupMembersRequest, FetchGroupMembersResponse>
{
    protected override uint Command => 0xfe7;

    protected override uint Service => 3;

    protected override Task<FetchGroupMembersRequest> ProcessRequest(FetchGroupMembersEventReq request, BotContext context)
    {
        return Task.FromResult(new FetchGroupMembersRequest
        {
            GroupUin = request.GroupUin,
            Field2 = 5,
            Field3 = 2,
            Body = new FetchGroupMembersRequestBody
            {
                MemberName = true,
                MemberCard = true,
                SpecialTitle = true,
                Level = true,
                JoinTimestamp = true,
                LastMsgTimestamp = true,
                ShutUpTimestamp = true,
                Permission = true,
            },
            Cookie = request.Cookie
        });
    }

    protected override async Task<FetchGroupMembersEventResp> ProcessResponse(FetchGroupMembersResponse response, BotContext context)
    {
        var group = await context.CacheContext.ResolveGroup(response.GroupUin);
        if (group == null) throw new InvalidTargetException(null, response.GroupUin);

        return new FetchGroupMembersEventResp(
            [.. response.Members.Select(raw => new BotGroupMember(
                group,
                raw.Id.Uin,
                raw.Id.Uid,
                raw.MemberName,
                (GroupMemberPermission)raw.Permission,
                (int)(raw.Level?.Level ?? 0),
                raw.MemberCard.MemberCard,
                raw.SpecialTitle,
                DateTimeOffset.FromUnixTimeSeconds(raw.JoinTimestamp).DateTime,
                DateTimeOffset.FromUnixTimeSeconds(raw.LastMsgTimestamp).DateTime,
                DateTimeOffset.FromUnixTimeSeconds(raw.ShutUpTimestamp).DateTime
            ))],
            response.Cookie
        );
    }
}