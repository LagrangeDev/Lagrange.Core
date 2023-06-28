using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.System;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(SsoAliveEvent))]
[Service("trpc.qq_new_tech.status_svc.StatusService.SsoHeartBeat")]
internal class SsoAliveService : BaseService<SsoAliveEvent>
{
    protected override bool Build(SsoAliveEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        using var stream = new MemoryStream();
        var packet = new NTSsoHeartBeat { Type = 1 };
        Serializer.Serialize(stream, packet);
        
        output = new BinaryPacket(stream.ToArray());
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SsoAliveEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = SsoAliveEvent.Result();
        extraEvents = null;
        return true;
    }
}