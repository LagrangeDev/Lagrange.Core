using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("send_like")]
public class SendLikeOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSendLike>(SerializerOptions.DefaultOptions) is { } like)
        {
            var Like =  await context.Like(like.UserId, like.Times ?? 1); // the times is ignored to simulate the real user behaviour
            return Like == "" ? new OneBotResult(null, 0, "ok") : new OneBotResult(Like, -1, "failed");
                // : new OneBotResult(null, 0, "user not found in the buffer");
        }

        throw new Exception();
    }
}
