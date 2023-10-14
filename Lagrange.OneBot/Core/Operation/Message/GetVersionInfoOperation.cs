using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_version_info")]
public class GetVersionInfoOperation : IOperation
{
    public Task<OneBotResult> HandleOperation(string echo, BotContext context, JsonObject? payload)
    {
        var appInfo = context.ContextCollection.AppInfo;
        string version = $"{appInfo.Os} | {appInfo.CurrentVersion}";
        
        return Task.FromResult(new OneBotResult(new OneBotVersionInfoResponse(version), 0, "ok", echo));
    }
}