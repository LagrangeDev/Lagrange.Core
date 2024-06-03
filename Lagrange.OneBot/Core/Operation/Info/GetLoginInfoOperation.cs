using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_login_info")]
public class GetLoginInfoOperation : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var keystore = context.ContextCollection.Keystore;
        var result = new OneBotLoginInfoResponse(keystore.Uin, keystore.Info?.Name ?? "");
        return Task.FromResult(new OneBotResult(result, 0, "ok"));
    }
}