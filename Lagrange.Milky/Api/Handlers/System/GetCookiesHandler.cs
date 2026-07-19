using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_cookies")]
public sealed class GetCookiesHandler : IApiHandler<GetCookiesHandler.Request, GetCookiesHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: get_cookies
    }

    public sealed class Request(string domain)
    {
        [JsonPropertyName("domain")] public required string Domain { get; init; } = domain;
    }

    public sealed class Result
    {
        [JsonPropertyName("cookies")] public required string Cookies { get; init; }
    }
}
