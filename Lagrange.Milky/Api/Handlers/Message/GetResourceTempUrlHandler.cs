using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_resource_temp_url")]
public sealed class GetResourceTempUrlHandler(BotContext lagrange) : IApiHandler<GetResourceTempUrlHandler.Request, GetResourceTempUrlHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        string url = await _lagrange.GetNTV2RichMediaUrl(request.ResourceId).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            Url = url,
        });
    }

    public sealed class Request(string resourceId)
    {
        [JsonPropertyName("resource_id")] public required string ResourceId { get; init; } = resourceId;
    }

    public sealed class Result
    {
        [JsonPropertyName("url")] public required string Url { get; init; }
    }
}
