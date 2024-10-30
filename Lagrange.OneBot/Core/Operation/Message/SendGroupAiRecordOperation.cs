using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_group_ai_record")]
public class GetAiRecordOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotGetAiRecord>(SerializerOptions.DefaultOptions);
        if (message != null)
        {
            var e = await context.GetGroupGenerateAiRecord(message.GroupId, message.Character, message.Text);
            if (e.code != 0 || e.recordEntity == null) return new OneBotResult(e.errMsg, e.code, "failed");


            var chain = MessageBuilder.Group(message.GroupId).Add(e.recordEntity).Build();
            var result = await context.SendMessage(chain);
            int hash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence ?? 0);

            return new OneBotResult(new OneBotMessageResponse(hash), (int)result.Result, "ok");
        }

        throw new Exception();
    }
}