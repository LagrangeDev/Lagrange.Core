using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Message;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_msg")]
public class GetMessageOperation(ContextBase database) : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        if (payload.Deserialize<OneBotGetMessage>() is { } getMsg)
        {
            var record = database.Query<MessageRecord>(x => x.MessageHash == getMsg.MessageId);
            var chain = (MessageChain)record;
            var elements = MessageService.Convert(chain);
            var response = new OneBotGetMessageResponse(chain.Time, chain.IsGroup ? "group" : "private", record.MessageHash, elements);
            
            return Task.FromResult(new OneBotResult(response, 200, "ok"));
        }
        
        throw new Exception();
    }
}