using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(AliveEvent))]
[Service("Heartbeat.Alive", 13)]
internal class AliveService : BaseService<AliveEvent>
{
    protected override bool Build(AliveEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        output = new BinaryPacket().WriteUint(4).ToArray();
        extraPackets = null;
        return true;
    }
}