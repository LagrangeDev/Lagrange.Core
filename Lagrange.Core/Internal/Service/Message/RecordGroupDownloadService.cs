using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(RecordGroupDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x126e_200")]
internal class RecordGroupDownloadService : BaseService<RecordGroupDownloadEvent>
{
    protected override bool Build(RecordGroupDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 4,
                    Command = 200
                },
                Scene = new SceneInfo
                {
                    RequestType = 1,
                    BusinessType = 3,
                    SceneType = 2,
                    Group = new GroupInfo { GroupUin = input.GroupUin }
                },
                Client = new ClientMeta { AgentType = 2 }
            },
            Download = new DownloadReq
            {
                Node = input.Node ?? new IndexNode
                {
                    FileUuid = input.FileUuid
                },
                Download = new DownloadExt
                {
                    Video = new VideoDownloadExt
                    {
                        BusiType = 0,
                        SceneType = 0
                    }
                }
            }
        }, 0x126e, 200, false, true);
        output = packet.Serialize();
        extraPackets = null;
        
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out RecordGroupDownloadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<NTV2RichMediaResp>>(input);
        var body = payload.Body.Download;
        string url = $"https://{body.Info.Domain}{body.Info.UrlPath}{body.RKeyParam}";
        
        output = RecordGroupDownloadEvent.Result((int)payload.ErrorCode, url);
        extraEvents = null;
        return true;
    }
}