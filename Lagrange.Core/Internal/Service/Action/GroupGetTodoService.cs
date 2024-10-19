using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupGetTodoEvent))]
[Service("OidbSvcTrpcTcp.0xf8e_1")]
internal class GroupGetTodoService : BaseService<GroupGetTodoEvent>
{
    protected override bool Build(GroupGetTodoEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xF8E_1>(new OidbSvcTrpcTcp0xF8E_1
        {
            GroupUin = input.GroupUin,
        }, 0xF8E, 1);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupGetTodoEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xF8E_1Response>>(input);

        output = packet.ErrorCode != 0
            ? GroupGetTodoEvent.Result((int)packet.ErrorCode, packet.ErrorMsg)
            : GroupGetTodoEvent.Result(
                (int)packet.ErrorCode,
                packet.ErrorMsg,
                packet.Body.Body.GroupUin,
                packet.Body.Body.Sequence,
                packet.Body.Body.Preview
            );
        extraEvents = null;
        return true;
    }
}