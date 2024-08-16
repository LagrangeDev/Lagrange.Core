using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("send_group_bot_callback")]
public class SetGroupBothdOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotSetGroupBothd>(SerializerOptions.DefaultOptions);

        if (message != null)
        {
            bool _ = await context.SetGroupBotHD(message.BotId, message.GroupId, message.Data_1, message.Data_2);

            return new OneBotResult(message.BotId, 0, "ok");
        }

        throw new Exception();
    }
}
