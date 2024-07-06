using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Notify;


namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_skey")]
public class GetSkeyOperation (TicketService ticket) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
            string? skey = await ticket.GetSKey();
            skey ??= "";
            return new OneBotResult(new JsonObject { { "skey", skey } }, 0, "ok");
    }
}