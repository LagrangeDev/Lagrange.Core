using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using FileInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.FileInfo;
using GroupInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.GroupInfo;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(ImageGroupUploadEvent))]
[Service("OidbSvcTrpcTcp.0x11c4_100")]
internal class ImageGroupUploadService : BaseService<ImageGroupUploadEvent>
{
    protected override bool Build(ImageGroupUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.Entity.ImageStream is null) throw new Exception();
        
        string md5 = input.Entity.ImageStream.Value.Md5(true);
        string sha1 = input.Entity.ImageStream.Value.Sha1(true);
        
        var buffer = new byte[1024]; // parse image header
        int _ = input.Entity.ImageStream.Value.Read(buffer.AsSpan());
        var type = ImageResolver.Resolve(buffer, out var size);
        string imageExt = type switch
        {
            ImageFormat.Jpeg => ".jpg",
            ImageFormat.Png => ".png",
            ImageFormat.Gif => ".gif",
            ImageFormat.Webp => ".webp",
            ImageFormat.Bmp => ".bmp",
            ImageFormat.Tiff => ".tiff",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        input.Entity.ImageStream.Value.Position = 0;

        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 1,
                    Command = 100
                },
                Scene = new SceneInfo
                {
                    RequestType = 2,
                    BusinessType = 1,
                    SceneType = 2,
                    Group = new GroupInfo { GroupUin = input.GroupUin }
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
                            FileSize = (uint)input.Entity.ImageStream.Value.Length,
                            FileHash = md5,
                            FileSha1 = sha1,
                            FileName = md5 + imageExt,
                            Type = new FileType
                            {
                                Type = 1,
                                PicFormat = (uint)type,
                                VideoFormat = 0,
                                VoiceFormat = 0
                            },
                            Width = (uint)size.X,
                            Height = (uint)size.Y,
                            Time = 0,
                            Original = 1
                        },
                        SubFileType = 0
                    }
                },
                TryFastUploadCompleted = true,
                SrvSendMsg = false,
                ClientRandomId = (ulong)Random.Shared.Next(),
                CompatQMsgSceneType = 2,
                ExtBizInfo = new ExtBizInfo
                {
                    Pic = new PicExtBizInfo
                    {
                        TextSummary = input.Entity.Summary!,
                        BytesPbReserveTroop = "0800180020004a00500062009201009a0100aa010c080012001800200028003a00".UnHex()
                    },
                    Video = new VideoExtBizInfo { BytesPbReserve = Array.Empty<byte>() },
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
        }, 0x11c4, 100, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ImageGroupUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<NTV2RichMediaResp>>(input);
        var upload = packet.Body.Upload;
        var compat = Serializer.Deserialize<CustomFace>(upload.CompatQMsg.AsSpan());
        
        output = ImageGroupUploadEvent.Result((int)packet.ErrorCode, upload.UKey, upload.MsgInfo, upload.IPv4s, compat);
        extraEvents = null;
        return true;
    }
}