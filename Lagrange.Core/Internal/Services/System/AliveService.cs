using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<AliveEvent>(Protocols.All)]
[Service("Heartbeat.Alive", RequestType.Simple, EncryptType.NoEncrypt, DisableLog = true)]
internal class AliveService : BaseService<AliveEvent, AliveEvent>
{
    private static readonly byte[] Buffer = [0x00, 0x00, 0x00, 0x04];
    
    protected override ValueTask<ReadOnlyMemory<byte>> Build(AliveEvent input, BotContext context)
    {
        return new ValueTask<ReadOnlyMemory<byte>>(Buffer);
    }

    protected override ValueTask<AliveEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        return new ValueTask<AliveEvent>(new AliveEvent());
    }
}