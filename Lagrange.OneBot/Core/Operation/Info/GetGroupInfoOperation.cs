using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_group_info")]
public class GetGroupInfoOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetGroupInfo>(SerializerOptions.DefaultOptions) is { } message)
        {
            var result = (await context.FetchGroups(message.NoCache)).FirstOrDefault(x => x.GroupUin == message.GroupId);

            if (result == null && !message.NoCache)
            {
                result = (await context.FetchGroups(true)).FirstOrDefault(x => x.GroupUin == message.GroupId);
            }

            return result == null
                ? new OneBotResult(null, -1, "failed")
                : new OneBotResult(new OneBotGroup(result), 0, "ok");
        }

        throw new Exception();
    }
}
