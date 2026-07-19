using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("recall_group_message")]
public sealed class RecallGroupMessageHandler : INoResultApiHandler<RecallGroupMessageHandler.Request>
{
    public ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        throw new global::System.NotImplementedException(); // TODO: api: recall_group_message
    }

    public sealed class Request(long groupId, long messageSeq)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
    }
}
