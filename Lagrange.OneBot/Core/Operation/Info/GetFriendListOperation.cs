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
        new((await context.FetchFriends(true)).Select(x => new OneBotFriend
        {
            UserId = x.Uin,
            NickName = x.Nickname,
            Remark = x.Remarks
        }).ToArray(), 0, "ok");
}