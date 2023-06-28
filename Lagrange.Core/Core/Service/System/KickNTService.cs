using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.System;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

// ReSharper disable once InconsistentNaming

[EventSubscribe(typeof(KickNTEvent))]
[Service("trpc.qq_new_tech.status_svc.StatusService.KickNT")]
internal class KickNTService : BaseService<KickNTEvent>
{
    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out KickNTEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var response = Serializer.Deserialize<ServiceKickNTResponse>(payload.AsSpan());
        output = KickNTEvent.Create(response.Tips, response.Title);
        
        extraEvents = null;
        return true;
    }
}