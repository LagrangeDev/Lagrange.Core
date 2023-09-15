using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.System;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

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