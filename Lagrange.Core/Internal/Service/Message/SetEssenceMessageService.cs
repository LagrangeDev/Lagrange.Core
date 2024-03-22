using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(SetEssenceMessageEvent))]
[Service("OidbSvcTrpcTcp.0xeac_1")]
internal class SetEssenceMessageService : BaseService<SetEssenceMessageEvent>
{
    protected override bool Build(SetEssenceMessageEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xEAC>(new OidbSvcTrpcTcp0xEAC
        {
            GroupUin = input.GroupUin,
            Sequence = input.Sequence,
            Random = input.Random
        }, 0xeac, 1);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SetEssenceMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<byte[]>>(input);

        output = SetEssenceMessageEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}