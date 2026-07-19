using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("send_group_message_reaction")]
public sealed class SendGroupMessageReactionHandler(BotContext lagrange) : INoResultApiHandler<SendGroupMessageReactionHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.SetGroupReaction(
            request.GroupId,
            (ulong)request.MessageSeq,
            request.Reaction,
            request.IsAdd
        ).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, long messageSeq, string reaction, string reactionType = "face", bool isAdd = true)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
        [JsonPropertyName("reaction")] public required string Reaction { get; init; } = reaction;
        [JsonPropertyName("reaction_type")] public string ReactionType { get; init; } = reactionType; // TODO: core does not implement reaction type
        [JsonPropertyName("is_add")] public bool IsAdd { get; init; } = isAdd;
    }
}
