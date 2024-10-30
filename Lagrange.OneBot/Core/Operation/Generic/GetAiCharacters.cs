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
        if (message == null) return new OneBotResult("unlucky", -1, "Failed");


        var e = await context.GetAiCharacters(message.GroupId);
        return e != null
            ? new OneBotResult(e.Select(x => new OneBotAiCharacter(x)), 0, "OK")
            : new OneBotResult("unlucky", -1, "Failed");
    }
}