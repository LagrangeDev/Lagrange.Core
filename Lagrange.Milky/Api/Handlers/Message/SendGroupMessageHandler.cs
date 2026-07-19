using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("send_group_message")]
public sealed class SendGroupMessageHandler : IApiHandler<SendGroupMessageHandler.Request, SendGroupMessageHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: send_group_message
    }

    public sealed class Request(long groupId/* , IReadOnlyList<OutgoingSegment> message */)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        // [JsonPropertyName("message")] public required IReadOnlyList<OutgoingSegment> Message { get; init; } = message;
    }

    public sealed class Result
    {
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
        [JsonPropertyName("time")] public required long Time { get; init; }
    }
}
