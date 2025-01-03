using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("send_poke")]
public class SendPokeOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSendPoke>(SerializerOptions.DefaultOptions) is { } poke)
        {
            if (poke.GroupId == null)
            {
                bool result = await context.FriendPoke(poke.UserId);
                return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
            }
            else
            {
                bool result = await context.GroupPoke(poke.GroupId.Value, poke.UserId);
                return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
            }
        }

        throw new Exception();
    }
}