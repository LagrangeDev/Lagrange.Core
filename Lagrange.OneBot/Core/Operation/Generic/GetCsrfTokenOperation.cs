using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_csrf_token")]
public class GetCsrfTokenOperation(TicketService ticket) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        int bkn = await ticket.GetCsrfToken();
        return new OneBotResult(new JsonObject { { "token", bkn } }, 0, "ok");
    }
}