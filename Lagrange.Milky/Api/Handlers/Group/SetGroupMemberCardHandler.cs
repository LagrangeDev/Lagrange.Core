using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("set_group_member_card")]
public sealed class SetGroupMemberCardHandler(BotContext lagrange) : INoResultApiHandler<SetGroupMemberCardHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupMemberRename(request.GroupId, request.UserId, request.Card).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, long userId, string card)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("card")] public required string Card { get; init; } = card;
    }
}
