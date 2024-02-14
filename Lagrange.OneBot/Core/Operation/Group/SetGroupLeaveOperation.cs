using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("set_group_leave")]
public class SetGroupLeaveOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        var message = payload.Deserialize<OneBotSetGroupLeave>();

        if (message != null)
        {
            bool _ = await context.LeaveGroup(message.GroupId); // TODO: Figure out is_dismiss
            return new OneBotResult(null, 200, "ok");
        }

        throw new Exception();
    }
}