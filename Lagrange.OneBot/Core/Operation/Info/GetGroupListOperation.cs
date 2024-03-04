using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_group_list")]
public class GetGroupListOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetGroupInfo>(SerializerOptions.DefaultOptions) is { } message)
        {
            var result = await context.FetchGroups(message.NoCache);
            return new OneBotResult(result.Select(x => new OneBotGroup(x)), 0, "ok");
        }
        else
        {
            var result = await context.FetchGroups();
            return new OneBotResult(result.Select(x => new OneBotGroup(x)), 0, "ok");
        }
    }
}
