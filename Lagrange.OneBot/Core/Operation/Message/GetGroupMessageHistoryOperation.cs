using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Message;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_group_msg_history")]
public class GetGroupMessageHistoryOperation(RealmHelper realm, MessageService message) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGroupMsgHistory>(SerializerOptions.DefaultOptions) is { } history)
        {
            var sequence = realm.Do(realm => history.MessageId == 0
                ? realm.All<MessageRecord>()
                    .Where(record => record.ToUinLong == history.GroupId)
                    .OrderByDescending(x => x.Time)
                    .First()
                    .Sequence
                : realm.All<MessageRecord>()
                    .First(record => record.Id == history.MessageId)
                    .Sequence);

            uint start = (uint)(sequence - (ulong)history.Count + 1);
            if (await context.GetGroupMessage(history.GroupId, start, (uint)sequence) is { } results)
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
