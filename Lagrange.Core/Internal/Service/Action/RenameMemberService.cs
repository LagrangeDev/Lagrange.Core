using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(RenameMemberEvent))]
[Service("OidbSvcTrpcTcp.0x8fc_3")]
internal class RenameMemberService : BaseService<RenameMemberEvent>
{
    protected override bool Build(RenameMemberEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8FC>(new OidbSvcTrpcTcp0x8FC
        {
            GroupUin = input.GroupUin,
            Body = new OidbSvcTrpcTcp0x8FCBody
            {
                TargetUid = input.TargetUid,
                TargetName = input.TargetName
            }
        }, 0x8fc, 3);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out RenameMemberEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x8FC_3Response>>(input);
        
        output = RenameMemberEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}