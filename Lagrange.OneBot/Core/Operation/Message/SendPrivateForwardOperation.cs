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
using Lagrange.OneBot.Utility;
using MessagePack;
using static Lagrange.Core.Message.MessageChain;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_private_forward_msg")]
public class SendPrivateForwardOperation(MessageCommon common, RealmHelper realm) : IOperation
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

            int obid = MessageRecord.CalcMessageHash(result.MessageId, result.Sequence ?? 0);

            realm.Do(realm => realm.Write(() => realm.Add(new MessageRecord
            {
                Id = obid,
                Type = MessageType.Friend,
                Sequence = result.Sequence ?? 0,
                ClientSequence = result.ClientSequence,
                MessageId = result.MessageId,
                Time = DateTimeOffset.FromUnixTimeSeconds(result.Timestamp),
                FromUin = context.BotUin,
                ToUin = chain.FriendUin,
                Entities = MessagePackSerializer.Serialize<List<IMessageEntity>>(chain, MessageRecord.OPTIONS)
            })));

            return new OneBotResult(new OneBotForwardResponse(obid, multi.ResId ?? ""), 0, "ok");
        }

        throw new Exception();
    }
}
