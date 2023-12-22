using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_group_msg")]
public sealed class SendGroupMessageOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        var result = payload.Deserialize<OneBotGroupMessageBase>() switch
        {
            OneBotGroupMessage message => await context.SendMessage(MessageCommon.ParseChain(message).Build()),
            OneBotGroupMessageSimple messageSimple => await context.SendMessage(MessageCommon.ParseChain(messageSimple).Build()),
            OneBotGroupMessageText messageText => await context.SendMessage(MessageCommon.ParseChain(messageText).Build()),
            _ => throw new Exception()
        };
        
        return new OneBotResult(new OneBotMessageResponse(0), 0, "ok");
    }
}