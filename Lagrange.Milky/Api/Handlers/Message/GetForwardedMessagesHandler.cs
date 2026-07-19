using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_forwarded_messages")]
public sealed class GetForwardedMessagesHandler : IApiHandler<GetForwardedMessagesHandler.Request, GetForwardedMessagesHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: get_forwarded_messages
    }

    public sealed class Request(string forwardId)
    {
        [JsonPropertyName("forward_id")] public required string ForwardId { get; init; } = forwardId;
    }

    public sealed class Result
    {
        // [JsonPropertyName("messages")] public required IReadOnlyList<IncomingForwardedMessage> Messages { get; init; }
    }
}
