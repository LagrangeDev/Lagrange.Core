using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(SetGroupRequestEvent))]
[Service("OidbSvcTrpcTcp.0x10c8_1")]
internal class SetGroupRequestService : BaseService<SetGroupRequestEvent>
{
    protected override bool Build(SetGroupRequestEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x10C8>(new OidbSvcTrpcTcp0x10C8
        {
            Accept = (uint)input.Operate,
            Body = new OidbSvcTrpcTcp0x10C8Body
            {
                Sequence = input.Sequence,
                EventType = input.Type,
                GroupUin = input.GroupUin,
                Message = input.Reason
            }
        }, 0x10c8, 1, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SetGroupRequestEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = SetGroupRequestEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}