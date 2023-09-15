using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Action;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupKickMemberEvent))]
[Service("OidbSvcTrpcTcp.0x8a0_1")]
internal class GroupKickMemberService : BaseService<GroupKickMemberEvent>
{
    protected override bool Build(GroupKickMemberEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8A0_1>(new OidbSvcTrpcTcp0x8A0_1
        {
            GroupUin = input.GroupUin,
            TargetUid = input.Uid,
            RejectAddRequest = input.RejectAddRequest,
            Field5 = ""
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupKickMemberEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x8A0_1Response>>(payload.AsSpan());
        
        output = GroupKickMemberEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}