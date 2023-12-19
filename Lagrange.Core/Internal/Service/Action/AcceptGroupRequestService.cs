using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(AcceptGroupRequestEvent))]
[Service("OidbSvcTrpcTcp.0x10c8_1")]
internal class AcceptGroupRequestService : BaseService<AcceptGroupRequestEvent>
{
    protected override bool Build(AcceptGroupRequestEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x10C8_1>(new OidbSvcTrpcTcp0x10C8_1
        {
            Accept = Convert.ToUInt32(!input.Accept) + 1,
            Body = new OidbSvcTrpcTcp0x10C8_1Body
            {
                LatestSeq = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 1) * 1000, // timestamp * 1000
                Field2 = 2,
                GroupUin = input.GroupUin,
                Field4 = ""
            }
        });
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }
    
    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out AcceptGroupRequestEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = AcceptGroupRequestEvent.Result(0);
        extraEvents = null;
        return true;
    }
}