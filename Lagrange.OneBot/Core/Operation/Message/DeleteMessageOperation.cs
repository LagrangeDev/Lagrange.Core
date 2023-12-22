using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("delete_msg")]
public class DeleteMessageOperation(ContextBase database) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        if (payload.Deserialize<OneBotGetMessage>() is { } getMsg)
        {
            var record = database.Query<MessageRecord>(x => x.MessageHash == getMsg.MessageId);
            var chain = (MessageChain)record;

            if (chain.IsGroup && await context.RecallGroupMessage(chain)) return new OneBotResult(null, 200, "ok");
        }

        throw new Exception();
    }
}