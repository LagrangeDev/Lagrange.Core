using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Action;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Action;

[EventSubscribe(typeof(RequestFriendSettingEvent))]
[Service("OidbSvcTrpcTcp.0x7c1_1")]
internal class RequestFriendSettingService : BaseService<RequestFriendSettingEvent>
{
    protected override bool Build(RequestFriendSettingEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x7C1_1>(new OidbSvcTrpcTcp0x7C1_1
        {
            Field1 = 1,
            SelfUin = keystore.Uin,
            TargetUin = input.TargetUin,
            Field4 = 3999,
            Field5 = 0
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream.ToArray());
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out RequestFriendSettingEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        
        Console.WriteLine(payload.Hex());

        output = RequestFriendSettingEvent.Result(0);
        extraEvents = null;
        return true;
    }
}