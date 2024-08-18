using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupRenameEvent))]
[Service("OidbSvcTrpcTcp.0x89a_15")]
internal class GroupRenameService : BaseService<GroupRenameEvent>
{
    protected override bool Build(GroupRenameEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x89A_15>(new OidbSvcTrpcTcp0x89A_15
        {
            GroupUin = input.GroupUin,
            Body = new OidbSvcTrpcTcp0x89A_15Body { TargetName = input.TargetName }
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupRenameEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        
        output = GroupRenameEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}