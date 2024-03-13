using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_status")]
public class GetStatusOperation : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        return Task.FromResult(new OneBotResult(new OneBotGetStatusResponse
        {
            Memory = GC.GetTotalMemory(false)
        }, 200, "ok"));
    }
}