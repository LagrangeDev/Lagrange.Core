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
            var chains = common.BuildForwardChains(context, forward, forward.GroupId);

            var multi = new MultiMsgEntity(forward.GroupId, [.. chains]);
            var chain = MessageBuilder.Group(forward.GroupId).Add(multi).Build();
            var result = await context.SendMessage(chain);

            if (!result.Sequence.HasValue || result.Sequence.Value == 0) return new OneBotResult(null, (int)result.Result, "failed");

            int hash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence.Value);

            return new OneBotResult(new OneBotForwardResponse(hash, multi.ResId ?? ""), 0, "ok");
        }

        throw new Exception();
    }
}
