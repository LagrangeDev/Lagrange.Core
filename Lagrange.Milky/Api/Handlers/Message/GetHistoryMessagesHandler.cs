using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Models;
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_history_messages")]
public sealed class GetHistoryMessagesHandler : IApiHandler<GetHistoryMessagesHandler.Request, GetHistoryMessagesHandler.Result>
{
    public ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: get_history_messages
    }

    public sealed class Request(string messageScene, long peerId, long? startMessageSeq, int limit = 20)
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; } = messageScene;
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; } = peerId;
        [JsonPropertyName("start_message_seq")] public long? StartMessageSeq { get; init; } = startMessageSeq;
        [JsonPropertyName("limit")] public int Limit { get; init; } = limit;
    }

    public sealed class Result
    {
        [JsonPropertyName("messages")] public required IReadOnlyList<IncomingMessageBase> Messages { get; init; }
        [JsonPropertyName("next_message_seq")] public required long? NextMessageSeq { get; init; }
    }
}
