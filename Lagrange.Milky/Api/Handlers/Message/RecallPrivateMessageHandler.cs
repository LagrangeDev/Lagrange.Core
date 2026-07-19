using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("recall_private_message")]
public sealed class RecallPrivateMessageHandler : INoResultApiHandler<RecallPrivateMessageHandler.Request>
{
    public ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: recall_private_message
    }

    public sealed class Request(long userId, long messageSeq)
    {
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
    }
}
