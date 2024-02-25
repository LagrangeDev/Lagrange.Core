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
                ? collection.Query().Where(x => x.FriendUin == history.UserId).OrderByDescending(x => x.Time).First() 
                : collection.FindOne(x => x.MessageHash == history.MessageId);
            var chain = (MessageChain)record;

            if (await context.GetRoamMessage(chain, 20) is { } results)
            {
                var messages = results
                    .Select(message.Convert)
                    .Select(x => message.ConvertToPrivateMsg(context.BotUin, chain, record.MessageHash))
                    .ToList();
                return new OneBotResult(new OneBotFriendMsgHistoryResponse(messages), 0, "ok");
            }
        }
        
        throw new Exception();
    }
}
