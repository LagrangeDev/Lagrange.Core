using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Request;

[Operation("set_group_add_request")]
public class SetGroupAddRequestOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetRequest>(SerializerOptions.DefaultOptions) is { } request)
        {
            string[] split = request.Flag.Split('-');
            ulong sequence = ulong.Parse(split[0]);
            uint groupUin = uint.Parse(split[1]);
            uint eventType = uint.Parse(split[2]);
            bool isFiltered = Convert.ToBoolean(uint.Parse(split.Length > 3 ? split[3] : "0"));
            var operate = request.Approve ? GroupRequestOperate.Allow : GroupRequestOperate.Deny;

            var logic = context.ContextCollection.Business.OperationLogic;
            bool result = isFiltered
                ? await logic.SetGroupFilteredRequest(groupUin, sequence, eventType, operate, request.Reason)
                : await logic.SetGroupRequest(groupUin, sequence, eventType, operate, request.Reason);
            return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
        }

        throw new Exception();
    }
}