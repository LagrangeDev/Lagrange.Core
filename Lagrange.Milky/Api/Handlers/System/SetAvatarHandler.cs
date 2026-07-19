using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("set_avatar")]
public sealed class SetAvatarHandler(BotContext lagrange, ResourceConverter resourceConverter) : INoResultApiHandler<SetAvatarHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly ResourceConverter _resourceConverter = resourceConverter;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        using var stream = await _resourceConverter.UriToStreamAsync(request.Uri, ct);
        return await _lagrange.SetBotAvatar(stream)
            ? new MilkyApiResponse()
            : new MilkyApiResponse(-500, "unknown error");
    }

    public sealed class Request(string uri)
    {
        [JsonPropertyName("uri")] public required string Uri { get; init; } = uri;
    }
}
