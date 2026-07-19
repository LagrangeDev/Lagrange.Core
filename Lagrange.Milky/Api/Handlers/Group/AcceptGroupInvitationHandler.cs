using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("accept_group_invitation")]
public sealed class AcceptGroupInvitationHandler(BotContext lagrange) : INoResultApiHandler<AcceptGroupInvitationHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.SetGroupNotification(
            request.GroupId,
            (ulong)request.InvitationSeq,
            BotGroupNotificationType.Invite,
            false,
            GroupNotificationOperate.Allow
        ).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long groupId, long invitationSeq)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("invitation_seq")] public required long InvitationSeq { get; init; } = invitationSeq;
    }
}
