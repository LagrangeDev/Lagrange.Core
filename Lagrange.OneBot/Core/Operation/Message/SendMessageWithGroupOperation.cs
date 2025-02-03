using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Message;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_msg_with_group")]
public class SendMessageWithGroupOperation(RealmHelper realm) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSendMessageWithGroup>(SerializerOptions.DefaultOptions) is { } history)
        {
            var record = realm.Do(realm => realm.All<MessageRecord>().First(record => record.Id == history.MessageId));
            var chain = (MessageChain)record;
            if (chain.GroupUin == 0)
            {
                return new OneBotResult("messageid not group", -1, "failed");
            }
            uint groupUin = chain.GroupUin ?? 0;

            if (await context.GetGroupMessageWithPushMsgBody(groupUin, (uint)chain.Sequence, (uint)chain.Sequence) is { } results)
            {
                var newMessageChain = new MessageChain(history.TargetGroupId);
                var sendMsgRes = await context.SendMessage(newMessageChain, results.Item2?[0] ?? throw new Exception("No PushMsgBody found"));

                if (sendMsgRes.Result != 0) return new OneBotResult(null, (int)sendMsgRes.Result, "failed");
                if (sendMsgRes.Sequence == null || sendMsgRes.Sequence == 0) return new OneBotResult(null, 9000, "failed");
                int hash = MessageRecord.CalcMessageHash(newMessageChain.MessageId, sendMsgRes.Sequence ?? 0);
                return new OneBotResult(new OneBotMessageResponse(hash, (int)sendMsgRes.Sequence.GetValueOrDefault()), (int)sendMsgRes.Result, "ok");
            }
        }
        throw new Exception();
    }
}