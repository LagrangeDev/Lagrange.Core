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

[EventSubscribe(typeof(GroupSetAdminEvent))]
[Service("OidbSvcTrpcTcp.0x1096_1")]
internal class GroupSetAdminService : BaseService<GroupSetAdminEvent>
{
    protected override bool Build(GroupSetAdminEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x1096_1>(new OidbSvcTrpcTcp0x1096_1
        {
            GroupUin = input.GroupUin,
            Uid = input.Uid,
            IsAdmin = input.IsAdmin
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupSetAdminEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x1096_1Response>>(payload.AsSpan());
        
        output = GroupSetAdminEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}