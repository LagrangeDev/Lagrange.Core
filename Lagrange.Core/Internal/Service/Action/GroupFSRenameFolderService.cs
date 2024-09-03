using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSRenameFolderEvent))]
[Service("OidbSvcTrpcTcp.0x6d7_2")]
internal class GroupFSRenameFolderService : BaseService<GroupFSRenameFolderEvent>
{
    protected override bool Build(GroupFSRenameFolderEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7_2>(new OidbSvcTrpcTcp0x6D7_2
        {
            Rename = new OidbSvcTrpcTcp0x6D7_2Rename
            {
                GroupUin = input.GroupUin,
                FolderId = input.FolderId,
                NewFolderName = input.NewFolderName
            }
        }, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupFSRenameFolderEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7Response>>(input);
        
        output = GroupFSRenameFolderEvent.Result(packet.Body.Rename.Retcode, packet.Body.Rename.RetMsg);
        extraEvents = null;
        return true;
    }
}