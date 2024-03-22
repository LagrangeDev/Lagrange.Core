using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

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
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out RequestFriendSettingEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = RequestFriendSettingEvent.Result(0);
        extraEvents = null;
        return true;
    }
}