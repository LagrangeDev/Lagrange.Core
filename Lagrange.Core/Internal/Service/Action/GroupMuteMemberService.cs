using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupMuteMemberEvent))]
[Service("OidbSvcTrpcTcp.0x1253_1")]
internal class GroupMuteMemberService : BaseService<GroupMuteMemberEvent>
{
    protected override bool Build(GroupMuteMemberEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x1253_1>(new OidbSvcTrpcTcp0x1253_1
        {
            GroupUin = input.GroupUin,
            Type = 1,
            Body = new OidbSvcTrpcTcp0x1253_1Body
            {
                TargetUid = input.Uid,
                Duration = input.Duration
            }
        });
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupMuteMemberEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x1253_1Response>>(input);
        
        output = GroupMuteMemberEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}