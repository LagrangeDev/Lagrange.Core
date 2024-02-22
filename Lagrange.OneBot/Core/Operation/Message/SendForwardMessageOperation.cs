using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
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
                if (((JsonElement)segment.Data).Deserialize<OneBotFakeNode>() is { } element)
                {
                    var chain = common.ParseFakeChain(element).Build();
                    chain.FriendInfo = new BotFriend(uint.Parse(element.Uin), string.Empty, element.Name, string.Empty, string.Empty);
                    chains.Add(chain);
                }
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