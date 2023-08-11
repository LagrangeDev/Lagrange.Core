using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(InfoSyncEvent))]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoInfoSync")]
internal class InfoSyncService : BaseService<InfoSyncEvent>
{
    protected override bool Build(InfoSyncEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new SsoInfoSync
        {
            SyncType = 143,
            RandomSeq = input.Random,
            Type = 2,
            GroupLastMsgTime = 0,
            C2C = new SsoInfoSyncC2C
            {
                Empty = Array.Empty<byte>(),
                C2CMsgLastTime = 0
            }
        };
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out InfoSyncEvent output, out List<ProtocolEvent>? extraEvents)
    {
        Console.WriteLine($"InfoSyncService.Parse: {input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix ).Hex()}");
        
        output = InfoSyncEvent.Result(0);
        extraEvents = null;
        return true;
    }
}