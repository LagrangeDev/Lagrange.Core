using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation;

public interface IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload);
}