using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupSetReactionEvent))]
[Service("OidbSvcTrpcTcp.0x9082_1")]
internal class GroupSetReactionService : BaseService<GroupSetReactionEvent>
{
    protected override bool Build(GroupSetReactionEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x9082_1>(new OidbSvcTrpcTcp0x9082_1
        {
            GroupUin = input.GroupUin,
            Sequence = input.Sequence,
            Code = input.Code,
            Field5 = true,
            Field6 = false,
            Field7 = false
        }, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupSetReactionEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        
        output = GroupSetReactionEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}