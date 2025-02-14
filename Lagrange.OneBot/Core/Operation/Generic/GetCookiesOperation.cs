using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_cookies")]
public class GetCookiesOperation (TicketService ticket) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["domain"]?.ToString() is { } domain)
        {
            var cookies = await ticket.GetCookies(domain);
            return new OneBotResult(new JsonObject { { "cookies", cookies } }, 0, "ok");
        }
        
        throw new Exception();
    }
}