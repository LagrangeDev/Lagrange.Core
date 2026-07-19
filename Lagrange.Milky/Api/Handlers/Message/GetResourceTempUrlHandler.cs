using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_resource_temp_url")]
public sealed class GetResourceTempUrlHandler : IApiHandler<GetResourceTempUrlHandler.Request, GetResourceTempUrlHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: get_resource_temp_url
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
