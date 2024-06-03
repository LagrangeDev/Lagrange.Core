using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_cookies")]
public class GetCookiesOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["domain"]?.ToString() is { } domain)
        {
            var cookies = await context.FetchCookies([domain]);
            return new OneBotResult(new JsonObject { { "cookies", cookies[0] } }, 0, "ok");
        }
        
        throw new Exception();
    }
}