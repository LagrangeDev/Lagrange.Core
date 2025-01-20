using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.Core.Internal.Event.System;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_rkey")]
public class FetchRkeyOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var fetchRKeyEvent = FetchRKeyEvent.Create();
        var events = await context.ContextCollection.Business.SendEvent(fetchRKeyEvent);
        var rKeyEvent = (FetchRKeyEvent)events[0];
        if (rKeyEvent.ResultCode != 0) return new OneBotResult(null, rKeyEvent.ResultCode, "failed");
        return new OneBotResult(new JsonObject { { "private_rkey", rKeyEvent.RKeys[0].Rkey }, { "group_rkey", rKeyEvent.RKeys[1].Rkey } }, 0, "ok");
    }
}