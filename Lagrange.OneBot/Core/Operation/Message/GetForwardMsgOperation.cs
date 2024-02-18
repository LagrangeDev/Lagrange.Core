using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_forward_msg")]
public class GetForwardMsgOperation(MessageService service) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetForwardMsg>() is { } forwardMsg)
        {
            var nodes = new List<OneBotSegment>();
            
            var @event = MultiMsgDownloadEvent.Create(context.ContextCollection.Keystore.Uid ?? "", forwardMsg.Id);
            var results = await context.ContextCollection.Business.SendEvent(@event);
            foreach (var chain in ((MultiMsgDownloadEvent)results[0]).Chains ?? throw new Exception())
            {
                var parsed = service.Convert(chain);
                var node = new OneBotNode(chain.FriendUin, "", parsed);
                nodes.Add(new OneBotSegment("node", node));
            }

            return new OneBotResult(new OneBotGetForwardMsgResponse(nodes), 0, "ok");
        }

        throw new Exception();
    }
}