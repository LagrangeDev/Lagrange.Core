using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_forward_msg")]
public class SendForwardMessageOperation(MessageCommon common) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotForward>() is { } forward)
        {
            List<MessageChain> chains = [];
            
            foreach (var segment in forward.Messages)
            {
                var element = ((JsonElement)segment.Data).Deserialize<OneBotFakeNode>();
                if (element is not null) chains.Add(common.ParseFakeChain(element).Build());
            }

            var @event = MultiMsgUploadEvent.Create(null, chains);
            var result = await context.ContextCollection.Business.SendEvent(@event);
            if (result.Count != 0 && result[0] is MultiMsgUploadEvent res)
            {
                return new OneBotResult(res.ResId, 0, "ok");
            }
            
            return new OneBotResult(null, 404, "failed");
        }

        throw new Exception();
    }
}