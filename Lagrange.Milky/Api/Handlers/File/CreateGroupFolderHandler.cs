using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.File;

[ApiHandler("create_group_folder")]
public sealed class CreateGroupFolderHandler(BotContext lagrange) : IApiHandler<CreateGroupFolderHandler.Request, CreateGroupFolderHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupFSCreateFolder(request.GroupId, request.FolderName);
        return new MilkyApiResponse<Result>(new Result
        {
            FolderId = "" // TODO: core no provide folder id
        });
    }

    public sealed class Request(long groupId, string folderName)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("folder_name")] public required string FolderName { get; init; } = folderName;
    }

    public sealed class Result
    {
        [JsonPropertyName("folder_id")] public required string FolderId { get; init; }
    }
}
