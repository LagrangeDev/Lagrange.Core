using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.System;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("get_rkey")]
public class GetRkey : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var fetchRKeyEvent = FetchRKeyEvent.Create();
        var events = await context.ContextCollection.Business.SendEvent(fetchRKeyEvent);
        var rKeyEvent = (FetchRKeyEvent)events[0];
        if (rKeyEvent.ResultCode != 0) return new OneBotResult(null, rKeyEvent.ResultCode, "failed");
        var response = new OneBotGetRkeyResponse(rKeyEvent.RKeys.Select(x => new OneBotRkey
            {
                Type = x.Type == 10
                    ? "private"
                    : "group",
                Rkey = x.Rkey,
                CreateTime = x.RkeyCreateTime,
                TtlSeconds = x.RkeyTtlSec
            })
            .ToList());
        return new OneBotResult(response, 0, "ok");
    }
}