using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("fetch_mface_key")]
internal class FetchMFaceKeyOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["emoji_ids"]?.Deserialize<string[]>() is { Length: not 0 } emojiIds)
        {
            if (await context.FetchMarketFaceKey([.. emojiIds]) is { Count: not 0 } keys)
            {
                return new(keys, 0, "ok");
            }
            else return new(Array.Empty<string>(), -1, "failed");
        }
        else throw new Exception();
    }
}
