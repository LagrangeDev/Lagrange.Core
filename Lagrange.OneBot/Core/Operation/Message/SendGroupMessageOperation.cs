using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_group_msg")]
public sealed class SendGroupMessageOperation(MessageCommon common) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var chain = payload.Deserialize<OneBotGroupMessageBase>() switch
        {
            OneBotGroupMessage message => common.ParseChain(message).Build(),
            OneBotGroupMessageSimple messageSimple => common.ParseChain(messageSimple).Build(),
            OneBotGroupMessageText messageText => common.ParseChain(messageText).Build(),
            _ => throw new Exception()
        };
        
        var result = await context.SendMessage(chain);
        int hash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence ?? 0);
        
        return new OneBotResult(new OneBotMessageResponse(hash), (int)result.Result, "ok");
    }
}