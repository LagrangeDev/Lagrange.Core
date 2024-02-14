using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_msg")]
public sealed class SendMessageOperation(MessageCommon common) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        var result = payload.Deserialize<OneBotMessageBase>() switch
        {
            OneBotMessage message => await context.SendMessage(common.ParseChain(message).Build()),
            OneBotMessageSimple messageSimple => await context.SendMessage(common.ParseChain(messageSimple).Build()),
            OneBotMessageText messageText => await context.SendMessage(common.ParseChain(messageText).Build()),
            _ => throw new Exception()
        };

        return new OneBotResult(new OneBotMessageResponse(0), 0, "ok");
    }
}