using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchFriendsEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xfd4_1")]
internal class FetchFriendsService : OidbService<FetchFriendsEventReq, FetchFriendsEventResp, IncPullRequest, IncPullResponse>
{
    protected override uint Command => 0xfd4;

    protected override uint Service => 1;

    protected override Task<IncPullRequest> ProcessRequest(FetchFriendsEventReq request, BotContext context)
    {
        return Task.FromResult(new IncPullRequest
        {
            ReqCount = 300,
            LocalSeq = 13,
            Cookie = request.Cookie,
            Flag = 1,
            ProxySeq = int.MaxValue,
            RequestBiz =
            [
                new IncPullRequestBiz { BizType = 1, BizData = new IncPullRequestBizBusi { ExtBusi = [103, 102, 20002, 27394, 20009, 20037] } },
                new IncPullRequestBiz { BizType = 4, BizData = new IncPullRequestBizBusi { ExtBusi = [100, 101, 102] } }
            ],
            ExtSnsFlagKey = [13578, 13579, 13573, 13572, 13568],
            ExtPrivateIdListKey = [4051]
        });
    }

    protected override Task<FetchFriendsEventResp> ProcessResponse(IncPullResponse response, BotContext context)
    {
        var friends = new List<BotFriend>();
        var categories = response.Category.Select(t => new BotFriendCategory(t.CategoryId, t.CategoryName, t.CategoryMemberCount, t.CatogorySortId)).ToList();
        var categoryMap = categories.ToDictionary(t => t.Id, t => t);

        foreach (var f in response.FriendList)
        {
            string nickname = f.SubBiz[1].Data[20002];
            string personalSign = f.SubBiz[1].Data[102];
            string remark = f.SubBiz[1].Data[103];
            string qid = f.SubBiz[1].Data[27394];

            var friend = new BotFriend(f.Uin, nickname, f.Uid, remark, personalSign, qid, categoryMap[f.CategoryId])
            {
                Age = f.SubBiz[1].NumData[20037],
                Gender = (BotGender)f.SubBiz[1].NumData[20009]
            };
            friends.Add(friend);
        }

        return Task.FromResult(new FetchFriendsEventResp(friends, categories, response.Cookie));
    }
}