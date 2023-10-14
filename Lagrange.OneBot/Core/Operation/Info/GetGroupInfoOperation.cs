using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_group_info")]
public class GetGroupInfoOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(string echo, BotContext context, JsonObject? payload)
    {
        if (payload.Deserialize<OneBotGetGroupInfo>() is { } message)
        {
            var result = await context.FetchGroups(message.NoCache);
            return new OneBotResult(result.FirstOrDefault(x => x.GroupUin == message.GroupId), 0, "ok", echo);
        }

        throw new Exception();
    }
}