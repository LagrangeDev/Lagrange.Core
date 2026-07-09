using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<PushParamsEvent>(Protocols.All)]
[Service("trpc.msg.register_proxy.RegisterProxy.PushParams")]
internal class PushParamsService : BaseService<PushParamsEvent, PushParamsEvent>
{
    protected override ValueTask<PushParamsEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var @params = ProtoHelper.Deserialize<PushParams>(input.Span);
        
        return ValueTask.FromResult(new PushParamsEvent());
    }
}