using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSCreateFolderEvent))]
[Service("OidbSvcTrpcTcp.0x6d7_0")]
internal class GroupFSCreateFolderService : BaseService<GroupFSCreateFolderEvent>
{
    protected override bool Build(GroupFSCreateFolderEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7_0>(new OidbSvcTrpcTcp0x6D7_0
        {
            Folder = new OidbSvcTrpcTcp0x6D7_0Folder
            {
                GroupUin = input.GroupUin,
                RootDirectory = "/",
                FolderName = input.Name
            }
        }, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupFSCreateFolderEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        
        output = GroupFSCreateFolderEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}