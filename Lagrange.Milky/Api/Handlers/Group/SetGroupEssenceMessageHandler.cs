using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Message;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Caching;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("set_group_essence_message")]
public sealed class SetGroupEssenceMessageHandler(BotContext lagrange, MessageCache cache) : INoResultApiHandler<SetGroupEssenceMessageHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MessageCache _cache = cache;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        var message = _cache.Get(MessageType.Group, request.GroupId, (ulong)request.MessageSeq)
            ?? (await _lagrange.GetGroupMessage(request.GroupId, (ulong)request.MessageSeq, (ulong)request.MessageSeq)
                .WaitAsync(ct))
                .FirstOrDefault();
        if (message == null) return new MilkyApiResponse(-404, "message not found");
        await _lagrange.SetEssenceMessage(message).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, long messageSeq, bool isSet = true)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
        [JsonPropertyName("is_set")] public bool IsSet { get; init; } = isSet;
    }
}
