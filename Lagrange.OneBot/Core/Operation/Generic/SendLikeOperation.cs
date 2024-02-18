using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("send_like")]
public class SendLikeOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSendLike>() is { } like)
        {
            return await context.Like(like.UserId) // the times is ignored to simulate the real user behaviour
                ? new OneBotResult(null, 0, "ok")
                : new OneBotResult(null, 0, "user not found in the buffer");
        }
        
        throw new Exception();
    }
}