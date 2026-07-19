using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.File;

[ApiHandler("move_group_file")]
public sealed class MoveGroupFileHandler(BotContext lagrange) : INoResultApiHandler<MoveGroupFileHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupFSMove(
            request.GroupId,
            request.FileId,
            request.TargetFolderId,
            request.ParentFolderId
        ).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, string fileId, string parentFolderId = "/", string targetFolderId = "/")
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("file_id")] public required string FileId { get; init; } = fileId;
        [JsonPropertyName("parent_folder_id")] public string ParentFolderId { get; init; } = parentFolderId;
        [JsonPropertyName("target_folder_id")] public string TargetFolderId { get; init; } = targetFolderId;
    }
}
