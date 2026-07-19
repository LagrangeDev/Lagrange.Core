using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.File;

[ApiHandler("rename_group_folder")]
public sealed class RenameGroupFolderHandler(BotContext lagrange) : INoResultApiHandler<RenameGroupFolderHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupFSRenameFolder(request.GroupId, request.FolderId, request.NewFolderName).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, string folderId, string newFolderName)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("folder_id")] public required string FolderId { get; init; } = folderId;
        [JsonPropertyName("new_folder_name")] public required string NewFolderName { get; init; } = newFolderName;
    }
}
