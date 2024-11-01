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
        var message = payload.Deserialize<OneBotGetAiCharacters>(SerializerOptions.DefaultOptions);
        if (message == null) throw new Exception();


        var (result, errMsg) = await context.GetAiCharacters(message.ChatType, message.GroupId);
        return result != null
            ? new OneBotResult(result.Select(x => new OneBotAiCharacters(x)), 0, "OK")
            : new OneBotResult(errMsg, -1, "Failed");
    }
}