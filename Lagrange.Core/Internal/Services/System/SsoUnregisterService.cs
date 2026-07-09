using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<SsoUnregisterEventReq>(Protocols.All)]
[Service("trpc.qq_new_tech.status_svc.StatusService.UnRegister")]
internal class SsoUnregisterService : BaseService<SsoUnregisterEventReq, SsoUnregisterEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(SsoUnregisterEventReq input, BotContext context)
    {
        var packet = new SsoUnregister
        {
            RegType = 1,
            DeviceInfo = new DeviceInfo(),
            UserTrigger = 1
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(packet));
    }

    protected override ValueTask<SsoUnregisterEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var packet = ProtoHelper.Deserialize<RegisterResponse>(input.Span);

        return ValueTask.FromResult(new SsoUnregisterEventResp(packet.Msg));
    }
}