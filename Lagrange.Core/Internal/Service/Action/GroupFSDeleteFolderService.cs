using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSDeleteFolderEvent))]
[Service("OidbSvcTrpcTcp.0x6d7_1")]
internal class GroupFSDeleteFolderService : BaseService<GroupFSDeleteFolderEvent>
{
    protected override bool Build(GroupFSDeleteFolderEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7_1>(new OidbSvcTrpcTcp0x6D7_1
        {
            Delete = new OidbSvcTrpcTcp0x6D7_1Delete
            {
                GroupUin = input.GroupUin,
                FolderId = input.FolderId
            }
        }, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupFSDeleteFolderEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7Response>>(input);
        
        output = GroupFSDeleteFolderEvent.Result(packet.Body.Delete.Retcode, packet.Body.Delete.RetMsg);
        extraEvents = null;
        return true;
    }
}