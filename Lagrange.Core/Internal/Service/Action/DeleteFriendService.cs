using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(DeleteFriendEvent))]
[Service("OidbSvcTrpcTcp.0x126b_0")]
internal class DeleteFriendService : BaseService<DeleteFriendEvent>
{
    protected override bool Build(DeleteFriendEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output,
        out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x126B_0>(new OidbSvcTrpcTcp0x126B_0
        {
            Field1 = new OidbSvcTrpcTcp0x126B_0_Field1 { TargetUid = input.TargetUid, Block = input.Block }
        },0x126b, 0, false, false);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out DeleteFriendEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = DeleteFriendEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}