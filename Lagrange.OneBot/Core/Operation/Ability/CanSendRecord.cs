using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Ability;

[Operation("can_send_record")]
public class CanSendRecord : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload) => 
        Task.FromResult(new OneBotResult(new JsonObject { { "yes", false } }, 0, "ok"));
}