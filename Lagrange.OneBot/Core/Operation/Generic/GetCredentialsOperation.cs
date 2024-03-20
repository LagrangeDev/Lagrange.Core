using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_credentials")]
public class GetCredentialsOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["domain"]?.ToString() is { } domain)
        {
            var cookies = await context.FetchCookies([domain]);
            int? bkn = null;
            if (await TicketHelper.GetSKey(context) is { } sKey) bkn = TicketHelper.GetCSRFToken(sKey);

            return new OneBotResult(new JsonObject
            {
                { "cookies", cookies[0] },
                { "csrf_token", bkn }
            }, 0, "ok");
        }

        throw new Exception();
    }
}