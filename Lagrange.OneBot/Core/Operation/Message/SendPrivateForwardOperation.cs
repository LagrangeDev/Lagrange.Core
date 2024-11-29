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

            var multi = new MultiMsgEntity([.. chains]);
            var chain = MessageBuilder.Friend(forward.UserId).Add(multi).Build();

            var result = await context.SendMessage(chain);

            if (result.Result != 0) return new OneBotResult(null, (int)result.Result, "failed");
            if (result.Sequence == null || result.Sequence == 0) return new OneBotResult(null, 9000, "failed");

            int hash = MessageRecord.CalcMessageHash(result.MessageId, result.Sequence ?? 0);

            database.GetCollection<MessageRecord>().Insert(hash, new()
            {
                FriendUin = context.BotUin,
                Sequence = result.Sequence ?? 0,
                ClientSequence = result.ClientSequence,
                Time = DateTimeOffset.FromUnixTimeSeconds(result.Timestamp).LocalDateTime,
                MessageId = result.MessageId,
                FriendInfo = new(
                    context.BotUin,
                    context.ContextCollection.Keystore.Uid ?? string.Empty,
                    context.BotName ?? string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                ),
                Entities = chain,
                MessageHash = hash,
                TargetUin = chain.FriendUin
            });

            return new OneBotResult(new OneBotForwardResponse(hash, multi.ResId ?? ""), 0, "ok");
        }

        throw new Exception();
    }
}
