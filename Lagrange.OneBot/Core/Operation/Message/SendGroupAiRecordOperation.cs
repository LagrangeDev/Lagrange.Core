using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_group_ai_record")]
public class SendGroupAiRecordOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotGetAiRecord>(SerializerOptions.DefaultOptions);
        if (message != null)
        {
            (int code, string _, var recordEntity) = await context.GetGroupGenerateAiRecord(
                message.GroupId,
                message.Character,
                message.Text,
                message.ChatType
            );
            return recordEntity != null && code == 0 
                ? new OneBotResult(new OneBotMessageResponse(0), code, "ok") 
                : new OneBotResult(null, code, "failed");
        }

        throw new Exception();
    }
}