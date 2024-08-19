using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Entity.File;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.File;

[Operation("get_group_file_url")]
public class GetGroupFileUrlOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetFileUrl>(SerializerOptions.DefaultOptions) is { } url)
        {
            string raw = await context.FetchGroupFSDownload(url.GroupId, url.FileId);
            return new OneBotResult(new JsonObject { { "url", raw } }, 0, "ok");
        }

        throw new Exception();
    }
}

[Operation("get_group_root_files")]
[Operation("get_group_files_by_folder")]
public class GetGroupRootFilesOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetFiles>(SerializerOptions.DefaultOptions) is { } file)
        {
            var entries = await context.FetchGroupFSList(file.GroupId, file.FolderId ?? "/");
            var files = new List<OneBotFile>();
            var folders = new List<OneBotFolder>();

            foreach (var entry in entries)
            {
                switch (entry)
                {
                    case BotFileEntry fileEntry:
                    {
                        var members = await context.FetchMembers(file.GroupId);
                        var member = members.FirstOrDefault(x => x.Uin == fileEntry.UploaderUin);
                        files.Add(new OneBotFile
                        {
                            GroupId = file.GroupId,
                            FileId = fileEntry.FileId,
                            FileName = fileEntry.FileName,
                            BusId = 0,
                            FileSize = fileEntry.FileSize,
                            UploadTime = fileEntry.UploadedTime.ToTimestamp(),
                            DeadTime = fileEntry.ExpireTime.ToTimestamp(),
                            ModifyTime = fileEntry.ModifiedTime.ToTimestamp(),
                            DownloadTimes = fileEntry.DownloadedTimes,
                            Uploader = fileEntry.UploaderUin,
                            UploaderName = member?.MemberName ?? string.Empty
                        });
                        break;
                    }
                    case BotFolderEntry folderEntry:
                    {
                        var members = await context.FetchMembers(file.GroupId);
                        var member = members.FirstOrDefault(x => x.Uin == folderEntry.CreatorUin);
                        folders.Add(new OneBotFolder
                        {
                            GroupId = file.GroupId,
                            FolderId = folderEntry.FolderId,
                            FolderName = folderEntry.FolderName,
                            CreateTime = folderEntry.CreateTime.ToTimestamp(),
                            Creator = folderEntry.CreatorUin,
                            CreatorName = member?.MemberName ?? string.Empty,
                            TotalFileCount = folderEntry.TotalFileCount
                        });
                        break;
                    }
                }
            }

            return new OneBotResult(new OneBotGetFilesResponse(files, folders), 0, "ok");
        }

        throw new Exception();
    }
}

[Operation("delete_group_file")]
public class DeleteGroupFileOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotDeleteFile>(SerializerOptions.DefaultOptions) is { } file)
        {
            await context.GroupFSDelete(file.GroupId, file.FileId);
            return new OneBotResult(null, 0, "ok");
        }

        throw new Exception();
    }
}

[Operation("create_group_file_folder")]
public class CreateGroupFileFolderOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotCreateFolder>(SerializerOptions.DefaultOptions) is { } folder)
        {
            await context.GroupFSCreateFolder(folder.GroupId, folder.Name);
            return new OneBotResult(null, 0, "ok");
        }

        throw new Exception();
    }
}