using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.File;

[ApiHandler("delete_group_folder")]
public sealed class DeleteGroupFolderHandler(BotContext lagrange) : INoResultApiHandler<DeleteGroupFolderHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupFSDeleteFolder(request.GroupId, request.FolderId).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, string folderId)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("folder_id")] public required string FolderId { get; init; } = folderId;
    }
}
