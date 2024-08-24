using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x6d6_2")]
internal class GroupFSDownloadService : BaseService<GroupFSDownloadEvent>
{
    protected override bool Build(GroupFSDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6>(new OidbSvcTrpcTcp0x6D6
        {
            Download = new OidbSvcTrpcTcp0x6D6Download
            {
                GroupUin = input.GroupUin,
                AppId = 7,
                BusId = 102,
                FileId = input.FileId
            }
        }, 0x6D6, 2, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupFSDownloadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6Response>>(input);
        var download = packet.Body.Download;

        string url = $"https://{download.DownloadDns}/ftn_handler/{download.DownloadUrl.Hex(true)}/?fname=";

        output = GroupFSDownloadEvent.Result((int)packet.ErrorCode, url);
        extraEvents = null;
        return true;
    }
}