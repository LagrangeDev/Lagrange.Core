using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Message;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Caching;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("recall_group_message")]
public sealed class RecallGroupMessageHandler(BotContext lagrange, MessageCache cache) : INoResultApiHandler<RecallGroupMessageHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MessageCache _cache = cache;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        var message = _cache.Get(MessageType.Group, request.GroupId, (ulong)request.MessageSeq)
            ?? (await _lagrange.GetGroupMessage(
                    request.GroupId,
                    (ulong)request.MessageSeq,
                    (ulong)request.MessageSeq
                ).WaitAsync(ct))
                .FirstOrDefault();
        if (message == null) return new MilkyApiResponse(-404, "Message not found");
        await _lagrange.RecallMessage(message).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, long messageSeq)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
    }
}
