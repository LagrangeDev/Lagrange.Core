using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_csrf_token")]
public class GetCSRFTokenOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (await TicketHelper.GetSKey(context) is { } sKey)
        {
            int bkn = TicketHelper.GetCSRFToken(sKey);
            return new OneBotResult(new JsonObject { { "token", bkn } }, 0, "ok");
        }
        
        return new OneBotResult(null, 400, "failed");
    }
}