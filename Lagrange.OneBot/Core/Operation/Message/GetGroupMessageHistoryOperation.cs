using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Message;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_group_msg_history")]
public class GetGroupMessageHistoryOperation(LiteDatabase database, MessageService message) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGroupMsgHistory>(SerializerOptions.DefaultOptions) is { } history)
        {
            var collection = database.GetCollection<MessageRecord>();
            var record = history.MessageId == 0
                ? collection.Find(x => x.GroupUin == history.GroupId).OrderByDescending(x => x.Time).First()
                : collection.FindById(history.MessageId);
            var chain = (MessageChain)record;

            if (await context.GetGroupMessage(history.GroupId, (uint)(chain.Sequence - history.Count + 1), chain.Sequence) is { } results)
            {
                var messages = results
                    .Select(x => message.ConvertToGroupMsg(context.BotUin, x))
                    .ToList();
                return new OneBotResult(new OneBotGroupMsgHistoryResponse(messages), 0, "ok");
            }
        }

        throw new Exception();
    }
}
