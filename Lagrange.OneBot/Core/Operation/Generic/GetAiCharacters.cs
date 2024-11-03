using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_ai_characters")]
public class GetAiCharacters : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotGetAiCharacters>(SerializerOptions.DefaultOptions)
            ?? throw new Exception();

        var (code, errMsg, result) = await context.GetAiCharacters(message.ChatType, message.GroupId);
        if (code != 0) return new(null, -1, "failed");

        return result != null
            ? new OneBotResult(result.Select(x => new OneBotAiCharacters(x)), 0, "ok")
            : new OneBotResult(null, -1, "failed");
    }
}