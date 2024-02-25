using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using FileInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.FileInfo;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(VideoDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x11e9_200")]
internal class VideoDownloadService : BaseService<VideoDownloadEvent>
{
    protected override bool Build(VideoDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = input.IsGroup ? 3u : 34u,
                    Command = 200
                },
                Scene = new SceneInfo
                {
                    RequestType = 2,
                    BusinessType = 2,
                    SceneType = 1,
                    C2C = new C2CUserInfo
                    {
                        AccountType = 2,
                        TargetUid = input.SelfUid
                    }
                },
                Client = new ClientMeta
                {
                    AgentType = 2 
                }
            },
            Download = new DownloadReq
            {
                Node = new IndexNode
                {
                    Info = new FileInfo
                    {
                        FileSize = 0,
                        FileHash = input.FileMd5,
                        FileSha1 = input.FileSha1 ?? "",
                        FileName = input.FileName,
                        Type = new FileType
                        {
                            Type = 2,
                            PicFormat = 0,
                            VideoFormat = 0,
                            VoiceFormat = 0
                        },
                        Width = 0,
                        Height = 0,
                        Time = 0,
                        Original = 0
                    },
                    FileUuid = input.Uuid,
                    StoreId = 0,
                    UploadTime = 0,
                    Ttl = 0,
                    SubType = 0
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
        }, 0x11e9, 200, false, true);
        output = packet.Serialize();
        extraPackets = null;
        
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out VideoDownloadEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<NTV2RichMediaResp>>(input.AsSpan());
        var body = payload.Body.Download;
        string url = $"https://{body.Info.Domain}{body.Info.UrlPath}{body.RKeyParam}";
        
        output = VideoDownloadEvent.Result((int)payload.ErrorCode, url);
        extraEvents = null;
        return true;
    }
}