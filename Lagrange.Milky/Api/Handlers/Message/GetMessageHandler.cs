using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Models;
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_message")]
public sealed class GetMessageHandler : IApiHandler<GetMessageHandler.Request, GetMessageHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: get_message
    }

    public sealed class Request(string messageScene, long peerId, long messageSeq)
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; } = messageScene;
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; } = peerId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
    }

    public sealed class Result
    {
        [JsonPropertyName("message")] public required IncomingMessageBase Message { get; init; }
    }
}
