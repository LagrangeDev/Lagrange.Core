using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.File;

[ApiHandler("get_group_file_download_url")]
public sealed class GetGroupFileDownloadUrlHandler(BotContext lagrange) : IApiHandler<GetGroupFileDownloadUrlHandler.Request, GetGroupFileDownloadUrlHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        string url = await _lagrange.GroupFSDownload(request.GroupId, request.FileId).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            DownloadUrl = url,
        });
    }

    public sealed class Request(long groupId, string fileId)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("file_id")] public required string FileId { get; init; } = fileId;
    }

    public sealed class Result
    {
        [JsonPropertyName("download_url")] public required string DownloadUrl { get; init; }
    }
}
