using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("send_private_message")]
public sealed class SendPrivateMessageHandler : IApiHandler<SendPrivateMessageHandler.Request, SendPrivateMessageHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: send_private_message
    }

    public sealed class Request(long userId/* , IReadOnlyList<OutgoingSegment> message */)
    {
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        // [JsonPropertyName("message")] public required IReadOnlyList<OutgoingSegment> Message { get; init; } = message;
    }

    public sealed class Result
    {
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
        [JsonPropertyName("time")] public required long Time { get; init; }
    }
}
