using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("set_group_member_special_title")]
public sealed class SetGroupMemberSpecialTitleHandler(BotContext lagrange) : INoResultApiHandler<SetGroupMemberSpecialTitleHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupSetSpecialTitle(request.GroupId, request.UserId, request.SpecialTitle).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, long userId, string specialTitle)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("special_title")] public required string SpecialTitle { get; init; } = specialTitle;
    }
}
