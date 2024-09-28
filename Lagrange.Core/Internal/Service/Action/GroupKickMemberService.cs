using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupKickMemberEvent))]
[Service("OidbSvcTrpcTcp.0x8a0_1")]
internal class GroupKickMemberService : BaseService<GroupKickMemberEvent>
{
    protected override bool Build(GroupKickMemberEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8A0_1>(new OidbSvcTrpcTcp0x8A0_1
        {
            GroupUin = input.GroupUin,
            TargetUid = input.Uid,
            RejectAddRequest = input.RejectAddRequest,
            Reason = input.Reason
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupKickMemberEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8A0_1Response>>(input);

        output = GroupKickMemberEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}