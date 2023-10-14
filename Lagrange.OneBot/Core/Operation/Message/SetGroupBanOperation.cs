using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("set_group_ban")]
public class SetGroupBanOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(string echo, BotContext context, JsonObject? payload)
    {
        var message = payload.Deserialize<OneBotSetGroupBan>();

        if (message != null)
        {
            bool _ = await context.MuteGroupMember(message.GroupId, message.UserId, message.Duration);
            return new OneBotResult(null, 0, "ok", echo);
        }

        throw new Exception();
    }
}