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
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_private_forward_msg")]
public class SendPrivateForwardOperation(MessageCommon common, LiteDatabase database) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotPrivateForward>(SerializerOptions.DefaultOptions) is { } forward)
        {
            var chains = common.BuildForwardChains(context, forward);

            var multi = new MultiMsgEntity(null, [.. chains]);
            var chain = MessageBuilder.Friend(forward.UserId).Add(multi).Build();
            var result = await context.SendMessage(chain);
            int hash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence ?? 0);

            database.GetCollection<MessageRecord>().Insert(hash, new()
            {
                FriendUin = context.BotUin,
                GroupUin = 0,
                Sequence = result.Sequence ?? 0,
                Time = chain.Time,
                MessageId = chain.MessageId,
                FriendInfo = new(
                    context.BotUin,
                    context.ContextCollection.Keystore.Uid ?? string.Empty,
                    context.BotName ?? string.Empty,
                    string.Empty,
                    string.Empty
                ),
                GroupMemberInfo = null,
                Entities = chain,
                MessageHash = hash
            });

            return new OneBotResult(new OneBotForwardResponse(hash, multi.ResId ?? ""), (int)result.Result, "ok");
        }

        throw new Exception();
    }
}
