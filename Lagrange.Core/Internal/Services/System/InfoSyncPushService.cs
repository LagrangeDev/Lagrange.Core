using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Proto.Nodes;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<InfoSyncPushEvent>(Protocols.All)]
[Service("trpc.msg.register_proxy.RegisterProxy.InfoSyncPush")]
internal class InfoSyncPushService : BaseService<InfoSyncPushEvent, InfoSyncPushEvent>
{
    protected override ValueTask<InfoSyncPushEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var obj = ProtoObject.Parse(input.Span);
        var push = ProtoHelper.Deserialize<InfoSyncPush>(input.Span);
        
        return ValueTask.FromResult(new InfoSyncPushEvent());
    }
}