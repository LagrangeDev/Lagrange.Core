using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_group_forward_msg")]
public class SendGroupForwardOperation(MessageCommon common) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGroupForward>(SerializerOptions.DefaultOptions) is { } forward)
        {
            var chains = common.BuildForwardChains(forward);

            var @event = MultiMsgUploadEvent.Create(null, chains);
            var result = await context.ContextCollection.Business.SendEvent(@event);
            if (result.Count == 0 || result[0] is not MultiMsgUploadEvent { ResId: not null } res)
            {
                return new OneBotResult(null, 404, "failed");
            }

            var chain = MessageBuilder.Group(forward.GroupId).Add(new MultiMsgEntity(res.ResId)).Build();
            var ret = await context.SendMessage(chain);
            int hash = MessageRecord.CalcMessageHash(chain.MessageId, ret.Sequence ?? 0);
            
            return new OneBotResult(new OneBotForwardResponse(hash, res.ResId), (int)ret.Result, "ok");
        }

        throw new Exception();
    }
}