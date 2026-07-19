using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("set_group_name")]
public sealed class SetGroupNameHandler(BotContext lagrange) : INoResultApiHandler<SetGroupNameHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.GroupRename(request.GroupId, request.NewGroupName).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, string newGroupName)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("new_group_name")] public required string NewGroupName { get; init; } = newGroupName;
    }
}
