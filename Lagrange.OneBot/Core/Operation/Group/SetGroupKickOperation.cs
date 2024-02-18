using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("set_group_kick")]
public class SetGroupKickOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotSetGroupKick>();

        if (message != null)
        {
            bool _ = await context.KickGroupMember(message.GroupId, message.UserId, message.RejectAddRequest);
            return new OneBotResult(null, 0, "ok");
        }

        throw new Exception();
    }
}