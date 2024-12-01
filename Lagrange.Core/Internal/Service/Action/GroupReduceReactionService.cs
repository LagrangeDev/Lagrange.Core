using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupReduceReactionEvent))]
[Service("OidbSvcTrpcTcp.0x9082_2")]
internal class GroupReduceReactionService : BaseService<GroupReduceReactionEvent>
{
    protected override bool Build(GroupReduceReactionEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x9082>(new OidbSvcTrpcTcp0x9082
        {
            GroupUin = input.GroupUin,
            Sequence = input.Sequence,
            Code = input.Code,
            Type = input.IsEmoji ? 2u : 1u,
            Field6 = false,
            Field7 = false
        }, 0x9082, 2, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupReduceReactionEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = GroupReduceReactionEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}