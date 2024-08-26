using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_friend_list")]
public class GetFriendListOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var friends = await context.FetchFriends(true);

        var friendGroups = await context.FetchFriendGroups(false);

        return new(friends.Select(friend => new OneBotFriend
        {
            UserId = friend.Uin,
            QId = friend.Qid,
            NickName = friend.Nickname,
            Remark = friend.Remarks,
            Group = new(friend.FriendGroupId, friendGroups[friend.FriendGroupId])
        }).ToArray(), 0, "ok");
    }
}