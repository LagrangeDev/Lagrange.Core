using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
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

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupMuteGlobalEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x89A_0Response>>(input);
        
        output = GroupMuteGlobalEvent.Result((int)(packet.ErrorCode));
        extraEvents = null;
        return true;
    }
}