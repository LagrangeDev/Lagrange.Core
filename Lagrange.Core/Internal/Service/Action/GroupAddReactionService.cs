using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupAddReactionEvent))]
[Service("OidbSvcTrpcTcp.0x9082_1")]
internal class GroupAddReactionService : BaseService<GroupAddReactionEvent>
{
    protected override bool Build(GroupAddReactionEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x9082>(new OidbSvcTrpcTcp0x9082
        {
            GroupUin = input.GroupUin,
            Sequence = input.Sequence,
            Code = input.Code,
            Field5 = true,
            Field6 = false,
            Field7 = false
        }, 0x9082, 1, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupAddReactionEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = GroupAddReactionEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}