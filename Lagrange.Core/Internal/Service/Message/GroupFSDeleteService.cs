using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[Service("OidbSvcTrpcTcp.0x6d6_3")]
[EventSubscribe(typeof(GroupFSDeleteEvent))]
internal class GroupFSDeleteService : BaseService<GroupFSDeleteEvent>
{
    protected override bool Build(GroupFSDeleteEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6>(new OidbSvcTrpcTcp0x6D6
        {
            Delete = new OidbSvcTrpcTcp0x6D6Delete
            {
                GroupUin = input.GroupUin,
                BusId = 102,
                FileId = input.FileId
            }
        }, 0x6D6, 3, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupFSDeleteEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6Response>>(input);
        
        output = GroupFSDeleteEvent.Result(packet.Body.Delete.RetCode, packet.Body.Delete.ClientWording);
        extraEvents = null;
        return true;
    }
}