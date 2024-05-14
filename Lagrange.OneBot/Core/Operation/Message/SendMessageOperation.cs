using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_msg")]
public sealed class SendMessageOperation(MessageCommon common, LiteDatabase database) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var chain = payload.Deserialize<OneBotMessageBase>(SerializerOptions.DefaultOptions) switch
        {
            OneBotMessage message => common.ParseChain(message).Build(),
            OneBotMessageSimple messageSimple => common.ParseChain(messageSimple).Build(),
            OneBotMessageText messageText => common.ParseChain(messageText).Build(),
            _ => throw new Exception()
        };

        var result = await context.SendMessage(chain);
        int hash = MessageRecord.CalcMessageHash(chain.MessageId, result.Sequence ?? 0);

        if (!chain.IsGroup) database.GetCollection<MessageRecord>().Insert(hash, new()
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

        return new OneBotResult(new OneBotMessageResponse(hash), (int)result.Result, "ok");
    }
}
