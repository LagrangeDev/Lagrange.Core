using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.File;

[Operation("upload_private_file")]
public class UploadPrivateFile : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotUploadPrivateFile>(SerializerOptions.DefaultOptions) is { } file)
        {
            var entity = new FileEntity(file.File);
            if (file.Name != null) entity.FileName = file.Name;

            bool result = await context.UploadFriendFile(file.UserId, entity);
            return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
        }
        
        throw new Exception();
    }
}