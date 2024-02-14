using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("set_group_card")]
public class SetGroupCardOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        var message = payload.Deserialize<OneBotSetGroupCard>();

        if (message != null)
        {
            bool _ = await context.RenameGroupMember(message.GroupId, message.UserId, message.Card);
            return new OneBotResult(null, 200, "ok");
        }

        throw new Exception();
    }
}