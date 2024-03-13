using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("delete_msg")]
public class DeleteMessageOperation(LiteDatabase database) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetMessage>(SerializerOptions.DefaultOptions) is { } getMsg)
        {
            var record = database.GetCollection<MessageRecord>().FindById(getMsg.MessageId);
            var chain = (MessageChain)record;

            if (chain.IsGroup && await context.RecallGroupMessage(chain)) return new OneBotResult(null, 0, "ok");
        }

        throw new Exception();
    }
}
