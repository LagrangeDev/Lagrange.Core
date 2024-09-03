using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSMoveEvent))]
[Service("OidbSvcTrpcTcp.0x6d6_5")]
internal class GroupFSMoveService : BaseService<GroupFSMoveEvent>
{
    protected override bool Build(GroupFSMoveEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6>(new OidbSvcTrpcTcp0x6D6
        {
            Move = new OidbSvcTrpcTcp0x6D6Move
            {
                GroupUin = input.GroupUin,
                AppId = 7,
                BusId = 102,
                fileId = input.FileId,
                ParentDirectory = input.ParentDirectory,
                TargetDirectory = input.TargetDirectory
            }
        }, 0x6D6, 5, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupFSMoveEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6Response>>(input);
        
        output = GroupFSMoveEvent.Result(packet.Body.Move.RetCode, packet.Body.Move.ClientWording);
        extraEvents = null;
        return true;
    }
}