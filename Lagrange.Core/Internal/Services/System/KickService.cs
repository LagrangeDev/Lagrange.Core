using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<KickEvent>(Protocols.All)]
[Service("trpc.qq_new_tech.status_svc.StatusService.KickNT")]
internal class KickService : BaseService<KickEvent, KickEvent>
{
    protected override ValueTask<KickEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var payload = ProtoHelper.Deserialize<KickNTReq>(input.Span);

        return ValueTask.FromResult(new KickEvent(payload.TipsTitle, payload.TipsInfo));
    }
}