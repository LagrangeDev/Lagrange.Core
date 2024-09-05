using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_credentials")]
public class GetCredentialsOperation(TicketService ticket) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["domain"]?.ToString() is { } domain)
        {
            var cookies = await context.FetchCookies([domain]);
            int bkn = await ticket.GetCsrfToken();

            return new OneBotResult(new JsonObject
            {
                { "cookies", cookies[0] },
                { "csrf_token", bkn }
            }, 0, "ok");
        }

        throw new Exception();
    }
}