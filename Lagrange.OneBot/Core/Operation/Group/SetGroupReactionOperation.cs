using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("set_group_reaction")]
public class SetGroupReactionOperation(LiteDatabase database) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetGroupReaction>(SerializerOptions.DefaultOptions) is { } data)
        {
            var message = (MessageChain)database.GetCollection<MessageRecord>().FindById(data.MessageId);

            bool result = await context.GroupSetMessageReaction(data.GroupId, message.Sequence, data.Code, data.IsAdd);
            return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
        }

        throw new Exception();
    }
}