using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_msg_with_group_seq")]
public class SendMessageWithGroupSeqOperation(MessageService message) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSendMessageWithGroupSeq>(SerializerOptions.DefaultOptions) is { } history)
        {
            if (await context.GetGroupMessageWithPushMsgBody(history.SourceGroupId, (uint)history.SourceMsgSeq, (uint)history.SourceMsgSeq) is { } results)
            {
                var newMessageChain = new MessageChain(history.TargetGroupId);
                var sendMsgRes = await context.SendMessage(newMessageChain, results.Item2?[0] ?? throw new Exception("No PushMsgBody found"));
                return new OneBotResult("www", (int)sendMsgRes.Result, "ok");
            }
        }

        throw new Exception();
    }
}