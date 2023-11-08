using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.Protocol.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_forward_msg")]
public class GetForwardMsgOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonObject? payload)
    {
        if (payload.Deserialize<OneBotGetForwardMsg>() is { } forwardMsg)
        {
            var nodes = new List<OneBotSegment>();
            
            var @event = MultiMsgDownloadEvent.Create(context.ContextCollection.Keystore.Uid ?? "", forwardMsg.Id);
            var results = await context.ContextCollection.Business.SendEvent(@event);
            foreach (var chain in ((MultiMsgDownloadEvent)results[0]).Chains ?? throw new Exception())
            {
                var parsed = MessageService.Convert(chain);
                var node = new OneBotNode(chain.FriendUin, "", parsed);
                nodes.Add(new OneBotSegment("node", node));
            }

            return new OneBotResult(new OneBotGetForwardMsgResponse(nodes), 0, "ok");
        }

        throw new Exception();
    }
}