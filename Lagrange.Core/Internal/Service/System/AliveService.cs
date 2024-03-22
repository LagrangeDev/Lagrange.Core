using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(AliveEvent))]
[Service("Heartbeat.Alive", 13)]
internal class AliveService : BaseService<AliveEvent>
{
    protected override bool Build(AliveEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        output = new BinaryPacket().WriteUint(4);
        extraPackets = null;
        return true;
    }
}