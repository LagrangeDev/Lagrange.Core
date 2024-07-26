using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_friend_list")]
public class GetFriendListOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload) =>

        new((await context.FetchFriends(true)).Select(friend =>
        {
            var task = context.FetchUserInfo(friend.Uin, true);
            task.Wait();
            var user = task.Result ?? throw new Exception("Are you kidding me?");

            return new OneBotFriend
            {
                UserId = friend.Uin,
                QId = user.Qid,
                NickName = friend.Nickname,
                Remark = friend.Remarks
            };
        }).ToArray(), 0, "ok");
}