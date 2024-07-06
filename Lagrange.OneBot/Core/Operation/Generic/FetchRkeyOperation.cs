using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_rkey")]
public class FetchRkeyOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var rkeys = await context.FetchRkey();
        return new OneBotResult(new JsonObject { { "private_rkey", rkeys[0] }, { "group_rkey", rkeys[1] } }, 0, "ok");
    }
}