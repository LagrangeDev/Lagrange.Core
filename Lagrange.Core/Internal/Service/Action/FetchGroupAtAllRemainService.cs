using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(FetchGroupAtAllRemainEvent))]
[Service("OidbSvcTrpcTcp.0x8a7_0")]
internal class FetchGroupAtAllRemainService : BaseService<FetchGroupAtAllRemainEvent>
{
    protected override bool Build(FetchGroupAtAllRemainEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8A7_0>(new OidbSvcTrpcTcp0x8A7_0    
        {
            SubCmd = 1,
            LimitIntervalTypeForUin = 2,
            LimitIntervalTypeForGroup = 1,
            Uin = keystore.Uin,
            GroupCode = input.GroupUin
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchGroupAtAllRemainEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8A7_0Response>>(input);
        output = FetchGroupAtAllRemainEvent.Result((int)packet.ErrorCode, packet.Body.RemainAtAllCountForUin, packet.Body.RemainAtAllCountForGroup);
        extraEvents = null;
        return true;
    }
}