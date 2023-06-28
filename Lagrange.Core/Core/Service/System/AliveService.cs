using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(AliveEvent))]
[Service("Heartbeat.Alive", 13)]
internal class AliveService : BaseService<AliveEvent>
{
    protected override bool Build(AliveEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        output = new BinaryPacket().WriteUint(4, false);
        extraPackets = null;
        return true;
    }
}