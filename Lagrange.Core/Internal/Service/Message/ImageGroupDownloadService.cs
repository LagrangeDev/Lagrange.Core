using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(ImageGroupDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x11c4_200")]
internal class ImageGroupDownloadService : BaseService<ImageGroupDownloadEvent>
{
    protected override bool Build(ImageGroupDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 1,
                    Command = 200
                },
                Scene = new SceneInfo
                {
                    RequestType = 2,
                    BusinessType = 1,
                    SceneType = 2,
                    Group = new GroupInfo { GroupUin = input.GroupUin }
                },
                Client = new ClientMeta { AgentType = 2 }
            },
            Download = new DownloadReq
            {
                Node = input.Node,
                Download = new DownloadExt
                {
                    Video = new VideoDownloadExt
                    {
                        BusiType = 0,
                        SceneType = 0
                    }
                }
            }
        }, 0x11c4, 200, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out ImageGroupDownloadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<NTV2RichMediaResp>>(input);
        var body = payload.Body.Download;
        string url = $"https://{body.Info.Domain}{body.Info.UrlPath}{body.RKeyParam}";
        
        output = ImageGroupDownloadEvent.Result((int)payload.ErrorCode, url);
        extraEvents = null;
        return true;
    }
}