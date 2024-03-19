using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_private_msg")]
public sealed class SendPrivateMessageOperation(LiteDatabase database, MessageCommon common) : IOperation {
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload) {
        var chain = payload.Deserialize<OneBotPrivateMessageBase>(SerializerOptions.DefaultOptions) switch {
            OneBotPrivateMessage message => common.ParseChain(message).Build(),
            OneBotPrivateMessageSimple messageSimple => common.ParseChain(messageSimple).Build(),
            OneBotPrivateMessageText messageText => common.ParseChain(messageText).Build(),
            _ => throw new Exception()
        };

        var result = await context.SendMessage(chain);

        MessageRecord record = new() {
            FriendUin = context.BotUin,
            Sequence = result.Sequence ?? 0,
            Time = DateTimeOffset.FromUnixTimeSeconds(result.Timestamp).DateTime,
            MessageId = chain.MessageId,
            FriendInfo = new BotFriend(
                context.BotUin,
                string.Empty,
                context.BotName ?? string.Empty,
                string.Empty,
                string.Empty
            ),
            Entities = chain,
            MessageHash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence ?? 0)
        };
        database.GetCollection<MessageRecord>().Insert(new BsonValue(record.MessageHash), record);

        return new OneBotResult(new OneBotMessageResponse(record.MessageHash), (int)result.Result, "ok");
    }
}
