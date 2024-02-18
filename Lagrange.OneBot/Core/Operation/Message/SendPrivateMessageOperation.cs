using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_private_msg")]
public sealed class SendPrivateMessageOperation(MessageCommon common) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var chain = payload.Deserialize<OneBotPrivateMessageBase>() switch
        {
            OneBotPrivateMessage message => common.ParseChain(message).Build(),
            OneBotPrivateMessageSimple messageSimple => common.ParseChain(messageSimple).Build(),
            OneBotPrivateMessageText messageText => common.ParseChain(messageText).Build(),
            _ => throw new Exception()
        };
        
        var result = await context.SendMessage(chain);
        int hash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence ?? 0);
        
        return new OneBotResult(new OneBotMessageResponse(hash), (int)result.Result, "ok");
    }
}