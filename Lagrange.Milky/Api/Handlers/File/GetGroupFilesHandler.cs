using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Api.Handlers.File;

[ApiHandler("get_group_files")]
public sealed class GetGroupFilesHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetGroupFilesHandler.Request, GetGroupFilesHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var entries = await _lagrange.FetchGroupFSList(request.GroupId, request.ParentFolderId).WaitAsync(ct);

        List<GroupFile> files = [];
        List<GroupFolder> folders = [];
        foreach (var entry in entries)
        {
            switch (entry)
            {
                case BotFileEntry file:
                {
                    files.Add(_converter.ToGroupFile(file, request.GroupId));
                    break;
                }
                case BotFolderEntry folder:
                {
                    folders.Add(_converter.ToGroupFolder(folder, request.GroupId));
                    break;
                }
            }
        }

        return new MilkyApiResponse<Result>(new Result
        {
            Files = files,
            Folders = folders,
        });
    }

    public sealed class Request(long groupId, string parentFolderId = "/")
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("parent_folder_id")] public string ParentFolderId { get; init; } = parentFolderId;
    }

    public sealed class Result
    {
        [JsonPropertyName("files")] public required IReadOnlyList<GroupFile> Files { get; init; }
        [JsonPropertyName("folders")] public required IReadOnlyList<GroupFolder> Folders { get; init; }
    }
}
