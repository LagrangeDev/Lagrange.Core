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
            FileEntity entity;
            if (file.File.StartsWith("base64://"))
            {
                var base64Data = file.File["base64://".Length..];
                var fileData = Convert.FromBase64String(base64Data);
                entity = new FileEntity(fileData, file.Name ?? "file");
            }
            else
            {
                entity = new FileEntity(file.File);
                if (file.Name != null) entity.FileName = file.Name;
            }
            
            var result = await context.UploadFriendFileWithResult(file.UserId, entity);
            return new OneBotResult(null, result.Retcode, result.Message ?? "ok");
        }

        throw new Exception();
    }
}