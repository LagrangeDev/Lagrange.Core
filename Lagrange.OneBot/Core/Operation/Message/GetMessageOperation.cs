using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Message;
using Lagrange.OneBot.Database;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_msg")]
public class GetMessageOperation(LiteDatabase database) : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        if (payload.Deserialize<OneBotGetMessage>() is { } getMsg)
        {
            var record = database.GetCollection<MessageRecord>().FindOne(x => x.MessageHash == getMsg.MessageId);
            var chain = (MessageChain)record;
            var elements = MessageService.Convert(chain);
            var response = new OneBotGetMessageResponse(chain.Time, chain.IsGroup ? "group" : "private", record.MessageHash, elements);
            
            return Task.FromResult(new OneBotResult(response, 200, "ok"));
        }
        
        throw new Exception();
    }
}