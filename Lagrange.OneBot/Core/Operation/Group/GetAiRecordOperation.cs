
using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("get_ai_record")]
public class GetAiRecordOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotGetAiRecord>(SerializerOptions.DefaultOptions);
        if (message != null)
        {
           var  e = await context.ContextCollection.Business.OperationLogic.GetAiRecordUrl(message.GroupId,message.Character,message.Text);
           return new OneBotResult(e, 0, "ok");
        }

        throw new Exception();
    }
}