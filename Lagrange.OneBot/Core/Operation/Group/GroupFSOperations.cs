using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("get_group_file_url")]
public class GetGroupFileUrlOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetFileUrl>() is { } url)
        {
            string raw = await context.FetchGroupFSDownload(url.GroupId, url.FileId);
            return new OneBotResult(new JsonObject { { "url", raw } }, 0, "ok");
        }

        throw new Exception();
    }
}