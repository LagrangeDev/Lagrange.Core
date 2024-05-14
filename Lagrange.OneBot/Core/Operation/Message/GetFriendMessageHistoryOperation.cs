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

[Operation("get_friend_msg_history")]
public class GetFriendMessageHistoryOperation(LiteDatabase database, MessageService message) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotFriendMsgHistory>(SerializerOptions.DefaultOptions) is { } history)
        {
            var collection = database.GetCollection<MessageRecord>();
            var record = history.MessageId == 0 
                ? collection.Find(x => x.FriendUin == history.UserId).OrderByDescending(x => x.Time).First() 
                : collection.FindById(history.MessageId);
            var chain = (MessageChain)record;

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
