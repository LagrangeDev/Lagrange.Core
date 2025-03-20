using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Notify;

namespace Lagrange.OneBot.Core.Operation.Request;

[Operation("get_group_requests")]
public class GetGroupRequestsOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var result = await context.FetchGroupRequestsWithResults();
        if (!result.IsSuccess) return new OneBotResult(null, result.Retcode, "failed");

        var requests = result.Data.Select(r =>
            {
                string? type = r.EventType switch
                {
                    Lagrange.Core.Common.Entity.BotGroupRequest.Type.GroupRequest => "add",
                    Lagrange.Core.Common.Entity.BotGroupRequest.Type.SelfInvitation or
                    Lagrange.Core.Common.Entity.BotGroupRequest.Type.GroupInvitation => "invite",
                    _ => null
                };
                if (type == null) return null;

                return new OneBotGroupRequest(
                    context.BotUin,
                    r.TargetMemberUin,
                    r.GroupUin,
                    type,
                    r.Comment,
                    $"{r.Sequence}-{r.GroupUin}-{(uint)r.EventType}-{Convert.ToInt32(r.IsFiltered)}"
                );
            })
            .Where(r => r != null);
        return new OneBotResult(requests, 0, "ok");
    }
}