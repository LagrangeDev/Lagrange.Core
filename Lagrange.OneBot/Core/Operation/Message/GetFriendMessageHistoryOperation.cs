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
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_friend_msg_history")]
public class GetFriendMessageHistoryOperation(RealmHelper realm, MessageService message) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotFriendMsgHistory>(SerializerOptions.DefaultOptions) is { } history)
        {
            var chain = realm.Do<MessageChain>(realm => history.MessageId == 0
                ? realm.All<MessageRecord>()
                    .Where(record => record.FromUinLong == history.UserId)
                    .OrderByDescending(record => record.Time)
                    .First()
                : realm.All<MessageRecord>()
                    .First(record => record.Id == history.MessageId));

            if (await context.GetRoamMessage(chain, history.Count) is { } results)
            {
                var messages = results
                    .Select(x => message.ConvertToPrivateMsg(context.BotUin, x))
                    .ToList();
                return new OneBotResult(new OneBotFriendMsgHistoryResponse(messages), 0, "ok");
            }
        }

        throw new Exception();
    }
}
