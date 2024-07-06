using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_cookies")]
public class GetCookiesOperation (TicketService ticket) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["domain"]?.ToString() is { } domain)
        {
            string cookies = await ticket.GetCookies(domain);
            return new OneBotResult(new JsonObject { { "cookies", cookies } }, 0, "ok");
        }
        
        throw new Exception();
    }
}