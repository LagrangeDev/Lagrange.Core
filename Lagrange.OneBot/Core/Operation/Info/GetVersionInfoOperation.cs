using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_version_info")]
public class GetVersionInfoOperation : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var appInfo = context.ContextCollection.AppInfo;
        string version = $"{appInfo.Os} | {appInfo.CurrentVersion}";
        
        return Task.FromResult(new OneBotResult(new OneBotVersionInfoResponse(version), 0, "ok"));
    }
}