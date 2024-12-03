using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.File;

[Operation("get_private_file_url")]
public class GetPrivateFileUrlOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetPrivateFile>(SerializerOptions.DefaultOptions) is { } url)
        {
            string raw = await context.FetchPrivateFSDownload(url.FileId, url.FileHash, url.UserId);
            return new OneBotResult(new JsonObject { { "url", raw } }, 0, "ok");
        }

        throw new Exception();
    }
}