using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
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
            var chains = common.BuildForwardChains(context, forward);

            var multi = new MultiMsgEntity(chains);
            var chain = MessageBuilder.Group(forward.GroupId).Add(multi).Build();
            var ret = await context.SendMessage(chain);

            if (ret.Result != 0) return new OneBotResult(null, (int)ret.Result, "failed");
            if (ret.Sequence == null || ret.Sequence == 0) return new OneBotResult(null, 9000, "failed");

            int hash = MessageRecord.CalcMessageHash(chain.MessageId, ret.Sequence ?? 0);

            return new OneBotResult(new OneBotForwardResponse(hash, multi.ResId ?? ""), (int)ret.Result, "ok");
        }

        throw new Exception();
    }
}
