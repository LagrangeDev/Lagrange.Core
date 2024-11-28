using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("delete_friend")]
public class DeleteFriendOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotDeleteFriend>(SerializerOptions.DefaultOptions) is {} delete)
        {
            return await context.DeleteFriend(delete.UserId, delete.Block)
                ? new OneBotResult(null, 0, "ok")
                : new OneBotResult(null, 0, "failed");
        }

        throw new Exception();
    }
}