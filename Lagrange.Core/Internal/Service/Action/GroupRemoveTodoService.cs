using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupRemoveTodoEvent))]
[Service("OidbSvcTrpcTcp.0xf90_3")]
internal class GroupRemoveTodoService : BaseService<GroupRemoveTodoEvent>
{
    protected override bool Build(GroupRemoveTodoEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xF90>(new OidbSvcTrpcTcp0xF90
        {
            GroupUin = input.GroupUin,
        }, 0xF90, 3);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupRemoveTodoEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = GroupRemoveTodoEvent.Result((int)packet.ErrorCode, packet.ErrorMsg);
        extraEvents = null;
        return true;
    }
}