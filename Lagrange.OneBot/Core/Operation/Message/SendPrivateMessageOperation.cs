using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_private_msg")]
public sealed class SendPrivateMessageOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        
        var result = payload.Deserialize<OneBotPrivateMessageBase>() switch
        {
            OneBotPrivateMessage message => await context.SendMessage(MessageCommon.ParseChain(message).Build()),
            OneBotPrivateMessageSimple messageSimple => await context.SendMessage(MessageCommon.ParseChain(messageSimple).Build()),
            OneBotPrivateMessageText messageText => await context.SendMessage(MessageCommon.ParseChain(messageText).Build()),
            _ => throw new Exception()
        };
        
        return new OneBotResult(new OneBotMessageResponse(0), 0, "ok");
    }
}