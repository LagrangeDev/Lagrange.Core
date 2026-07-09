using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<SetGroupNotificationEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x10c8_1")]
internal class SetGroupNotificationService : OidbService<SetGroupNotificationEventReq, SetGroupNotificationEventResp, SetGroupNotificationRequest, SetGroupNotificationResponse>
{
    protected override uint Command => 0x10c8;

    protected override uint Service => 1;

    protected override Task<SetGroupNotificationRequest> ProcessRequest(SetGroupNotificationEventReq request, BotContext context)
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

    protected override Task<SetGroupNotificationEventResp> ProcessResponse(SetGroupNotificationResponse response, BotContext context)
    {
        return Task.FromResult(SetGroupNotificationEventResp.Default);
    }
}
