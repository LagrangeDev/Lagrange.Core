using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Action;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Packets.Service.Oidb.Resopnse;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Action;

[EventSubscribe(typeof(GroupMuteMemberEvent))]
[Service("OidbSvcTrpcTcp.0x1253_1")]
internal class GroupMuteMemberService : BaseService<GroupMuteMemberEvent>
{
    protected override bool Build(GroupMuteMemberEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
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
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupMuteMemberEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x1253_1Response>>(payload.AsSpan());
        
        output = GroupMuteMemberEvent.Result(packet.Body.Success == "success" ? 0 : 1);
        extraEvents = null;
        return true;
    }
}