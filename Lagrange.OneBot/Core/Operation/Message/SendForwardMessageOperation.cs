using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_forward_msg")]
public class SendForwardMessageOperation(MessageCommon common) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotForward>(SerializerOptions.DefaultOptions) is { } forward)
        {
            var chains = common.BuildForwardChains(forward);

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
