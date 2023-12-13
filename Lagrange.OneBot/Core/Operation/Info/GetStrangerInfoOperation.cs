namespace Lagrange.OneBot.Core.Operation.Info;

using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;

[Operation("get_stranger_info")]
public class GetStrangerInfoOperation : IOperation
{
    private static int initCacheFriend = 0;
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        if (payload.Deserialize<OneBotGetStrangerInfo>() is { } message)
        {
            if(initCacheFriend == 0)
            {
                await context.FetchFriends();
                initCacheFriend = 1;
            }
            var result = await context.FetchStranger(message.UserId, message.NoCache);
            return new OneBotResult(new OneBotStranger(result.Uin, result.Nickname), 0, "ok");
        }

        throw new Exception();
    }
}