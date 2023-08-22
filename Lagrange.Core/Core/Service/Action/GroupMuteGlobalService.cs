using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Action;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Packets.Service.Oidb.Response;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf.Meta;

namespace Lagrange.Core.Core.Service.Action;

[EventSubscribe(typeof(GroupMuteGlobalEvent))]
[Service("OidbSvcTrpcTcp.0x89a_0")]
internal class GroupMuteGlobalService : BaseService<GroupMuteGlobalEvent>
{
    private static readonly RuntimeTypeModel Serializer = RuntimeTypeModel.Create();

    static GroupMuteGlobalService() => Serializer.UseImplicitZeroDefaults = false; // 666

    protected override bool Build(GroupMuteGlobalEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x89A_0>(new OidbSvcTrpcTcp0x89A_0
        {
            GroupUin = input.GroupUin,
            State = new OidbSvcTrpcTcp0x89A_0State { S = input.IsMute ? uint.MaxValue : 0 }
        });

        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupMuteGlobalEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x89A_0Response>>(payload.AsSpan());
        
        output = GroupMuteGlobalEvent.Result(packet.Body.ErorMsg == null ? 0 : -1);
        extraEvents = null;
        return true;
    }
}