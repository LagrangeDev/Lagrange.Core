using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using FileInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.FileInfo;
using GroupInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.GroupInfo;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(VideoUploadEvent))]
[Service("OidbSvcTrpcTcp.0x11e9_100")]
internal class VideoUploadService : BaseService<VideoUploadEvent>
{
    protected override bool Build(VideoUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.Entity.VideoStream is null) throw new Exception();
        
        string md5 = input.Entity.VideoStream.Md5(true);
        string sha1 = input.Entity.VideoStream.Sha1(true);
        
        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 3,
                    Command = 100
                },
                Scene = new SceneInfo
                {
                    RequestType = 2,
                    BusinessType = 2,
                    SceneType = 1,
                    C2C = new C2CUserInfo
                    {
                        AccountType = 2,
                        TargetUid = input.TargetUid
                    }
                },
                Client = new ClientMeta { AgentType = 2 },
            },
            Upload = new UploadReq
            {
                UploadInfo = new List<UploadInfo>
                {
                    new()
                    {
                        FileInfo = new FileInfo
                        {
                            FileSize = (uint)input.Entity.VideoStream.Length,
                            FileHash = md5,
                            FileSha1 = sha1,
                            FileName = "video.mp4",
                            Type = new FileType
                            {
                                Type = 2,
                                PicFormat = 0,
                                VideoFormat = 0,
                                VoiceFormat = 0
                            },
                            Width = 0,
                            Height = 0,
                            Time = (uint)input.Entity.VideoLength,
                            Original = 0
                        },
                        SubFileType = 0
                    },
                    new()
                    {
                        FileInfo = new FileInfo  // dummy images
                        {
                            FileSize = (uint)input.Entity.VideoStream.Length - 1200,
                            FileHash = md5,
                            FileSha1 = sha1,
                            FileName = "video.jpg",
                            Type = new FileType
                            {
                                Type = 1,
                                PicFormat = 0,
                                VideoFormat = 0,
                                VoiceFormat = 0
                            },
                            Width = 1920,
                            Height = 1080,
                            Time = 0,
                            Original = 0
                        },
                        SubFileType = 100
                    },
                },
                TryFastUploadCompleted = true,
                SrvSendMsg = false,
                ClientRandomId = (ulong)Random.Shared.Next(),
                CompatQMsgSceneType = 2,
                ExtBizInfo = new ExtBizInfo
                {
                    Pic = new PicExtBizInfo { BizType = 0, TextSummary = "" },
                    Video = new VideoExtBizInfo { BytesPbReserve = "800100".UnHex() },
                    Ptt = new PttExtBizInfo
                    {
                        BytesReserve = Array.Empty<byte>(),
                        BytesPbReserve = Array.Empty<byte>(),
                        BytesGeneralFlags = Array.Empty<byte>()
                    }
                },
                ClientSeq = 0,
                NoNeedCompatMsg = false
            }
        }, 0x11e9, 100, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out VideoUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<NTV2RichMediaResp>>(input);
        var upload = packet.Body.Upload;
        var compat = Serializer.Deserialize<VideoFile>(packet.Body.Upload.CompatQMsg.AsSpan());
        
        output = VideoUploadEvent.Result((int)packet.ErrorCode, upload.UKey, upload.MsgInfo, upload.IPv4s, compat);
        extraEvents = null;
        return true;
    }
}