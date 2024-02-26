using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupTransferEvent))]
[Service("OidbSvcTrpcTcp.0x89e_0")]
internal class GroupTransferService : BaseService<GroupTransferEvent>
{
    protected override bool Build(GroupTransferEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x89E_0>(new OidbSvcTrpcTcp0x89E_0
        {
            GroupUin = input.GroupUin,
            SourceUid = input.SourceUid,
            TargetUid = input.TargetUid
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out GroupTransferEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<byte[]>>(input.AsSpan());
        
        output = GroupTransferEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}