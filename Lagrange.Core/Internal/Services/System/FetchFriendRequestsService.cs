using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchFriendRequestsEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x5cf_11")]
internal class FetchFriendRequestsService : OidbService<FetchFriendRequestsEventReq, FetchFriendRequestsEventResp, D5CFReqBody, D5CFRspBody>
{
    protected override uint Command => 0x5cf;
    protected override uint Service => 11;

    protected override Task<D5CFReqBody> ProcessRequest(FetchFriendRequestsEventReq request, BotContext context)
    {
        return Task.FromResult(new D5CFReqBody
        {
            Version = 1,
            Type = 6,
            SelfUid = context.Keystore.Uid,
            StartIndex = 0,
            RequestCount = 80,
            GetFlag = 2,
            StartTime = 0,
            NeedCommonFriend = 1,
            Field22 = 1
        });
    }

    protected override Task<FetchFriendRequestsEventResp> ProcessResponse(D5CFRspBody response, BotContext context)
    {
        var success = response.SuccessRead ?? throw new OperationException(-1, "Friend request list was not returned");
        var requests = success.All?.Select(request => new BotFriendRequest(
            ResolveUin(request.SelfUid, context),
            ResolveUin(request.FriendUid, context),
            request.State,
            request.Comment ?? string.Empty,
            request.AddSource ?? string.Empty,
            request.Time)).ToList() ?? [];

        return Task.FromResult(new FetchFriendRequestsEventResp(requests));
    }

    private static long ResolveUin(string uid, BotContext context) =>
        uid == context.Keystore.Uid ? context.BotUin : context.CacheContext.ResolveUin(uid);
}
