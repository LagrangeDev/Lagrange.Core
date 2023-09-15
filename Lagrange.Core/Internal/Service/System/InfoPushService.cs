using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.System;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(InfoPushGroupEvent))]
[Service("trpc.msg.register_proxy.RegisterProxy.InfoSyncPush")]
internal class InfoPushService : BaseService<InfoPushGroupEvent>
{
    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out InfoPushGroupEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        
        var data = Serializer.Deserialize<SsoInfoPushData>(payload.AsSpan());
        if (data.Type == 5)
        {
            var packet = Serializer.Deserialize<SsoInfoPush<List<InfoPushGroup>>>(payload.AsSpan());
            
            var groups = new List<BotGroup>();
            if (packet.Info != null) groups.AddRange(packet.Info.Select(raw => new BotGroup(raw.GroupUin, raw.GroupName)));

            output = InfoPushGroupEvent.Result(groups);
            extraEvents = null;
            return true;
        }

        output = null!;
        extraEvents = null;
        return false;
    }
}