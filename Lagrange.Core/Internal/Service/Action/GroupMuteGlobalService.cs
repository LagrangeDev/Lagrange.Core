using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Action;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf.Meta;

namespace Lagrange.Core.Internal.Service.Action;

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
        
        output = GroupMuteGlobalEvent.Result((int)(packet.ErrorCode));
        extraEvents = null;
        return true;
    }
}