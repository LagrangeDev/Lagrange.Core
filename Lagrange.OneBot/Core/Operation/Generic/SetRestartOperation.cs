using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("set_restart")]
public class SetRestartOperation(IHost host) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        try
        {
            return new OneBotResult(null, 0, "ok");
        }
        finally
        {
            await host.StopAsync();
            await host.StartAsync();
        }
    }
}