using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Request;

[Operation("set_group_add_request")]
public class SetGroupAddRequestOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetRequest>() is { } request)
        {
            string[] split = request.Flag.Split('-');
            ulong sequence = ulong.Parse(split[0]);
            uint groupUin = uint.Parse(split[1]);
            uint eventType = uint.Parse(split[2]);

            bool result = await context.ContextCollection.Business.OperationLogic.SetGroupRequest(groupUin, sequence, eventType, request.Approve);
            return new OneBotResult(null, result ? 0 : 1, "ok");
        }

        throw new Exception();
    }
}