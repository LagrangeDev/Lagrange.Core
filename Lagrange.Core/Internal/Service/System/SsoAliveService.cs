using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(SsoAliveEvent))]
[Service("trpc.qq_new_tech.status_svc.StatusService.SsoHeartBeat")]
internal class SsoAliveService : BaseService<SsoAliveEvent>
{
    protected override bool Build(SsoAliveEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        using var stream = new MemoryStream();
        var packet = new NTSsoHeartBeat { Type = 1 };
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SsoAliveEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = SsoAliveEvent.Result();
        extraEvents = null;
        return true;
    }
}