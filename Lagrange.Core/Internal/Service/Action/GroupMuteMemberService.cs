using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

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

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupMuteMemberEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x1253_1Response>>(input.AsSpan());
        
        output = GroupMuteMemberEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}