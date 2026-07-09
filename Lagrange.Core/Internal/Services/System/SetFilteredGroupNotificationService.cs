using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<SetFilteredGroupNotificationEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x10c8_2")]
internal class SetFilteredGroupNotificationService : OidbService<SetFilteredGroupNotificationEventReq, SetFilteredGroupNotificationEventResp, SetGroupNotificationRequest, SetGroupNotificationResponse>
{
    protected override uint Command => 0x10c8;

    protected override uint Service => 2;

    protected override Task<SetGroupNotificationRequest> ProcessRequest(SetFilteredGroupNotificationEventReq request, BotContext context)
    {
        return Task.FromResult(new SetGroupNotificationRequest
        {
            Operate = (ulong)request.Operate,
            Body = new SetGroupNotificationRequestBody
            {
                Sequence = request.Sequence,
                Type = (ulong)request.Type,
                GroupUin = request.GroupUin,
                Message = request.Message,
            }
        });
    }

    protected override Task<SetFilteredGroupNotificationEventResp> ProcessResponse(SetGroupNotificationResponse response, BotContext context)
    {
        return Task.FromResult(SetFilteredGroupNotificationEventResp.Default);
    }
}
