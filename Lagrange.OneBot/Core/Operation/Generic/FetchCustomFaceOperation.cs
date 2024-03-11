using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("fetch_custom_face")]
internal class FetchCustomFaceOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload) => 
        new(await context.FetchCustomFace(), 0, "ok");
}