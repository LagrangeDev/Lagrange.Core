using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("mark_message_as_read")]
public sealed class MarkMessageAsReadHandler : INoResultApiHandler<MarkMessageAsReadHandler.Request>
{
    public ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: mark_message_as_read
    }

    public sealed class Request(string messageScene, long peerId, long messageSeq)
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; } = messageScene;
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; } = peerId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
    }
}
