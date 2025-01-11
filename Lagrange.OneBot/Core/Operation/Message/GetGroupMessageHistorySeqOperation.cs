using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Message;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_group_msg_history_seq")]
public class GetGroupMessageHistorySeqOperation(MessageService message) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGroupMsgHistorySeq>(SerializerOptions.DefaultOptions) is { } history)
        {
            if (await context.GetGroupMessage(history.GroupId, (uint)history.Start, (uint)(history.End == 0 ? history.Start : history.End)) is { } results)
            {
                var messages = results
                    .Select(x => message.ConvertToGroupMsg(context.BotUin, x))
                    .ToList();
                return new OneBotResult(new OneBotGroupMsgHistoryResponse(messages), 0, "ok");
            }
        }

        throw new Exception();
    }
}
