using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(ImageGroupUploadEvent))]
[Service("ImgStore.GroupPicUp")]
internal class ImageGroupUploadService : BaseService<ImageGroupUploadEvent>
{
    protected override bool Build(ImageGroupUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        input.Stream.Seek(0, SeekOrigin.Begin);
        
        var buffer = new byte[1024]; // parse image header
        int _ = input.Stream.Read(buffer.AsSpan());
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
        
        var packet = new GroupPicUp<GroupPicUpRequest>
        {
            NetType = 3,
            SubCmd = 1,
            Body = new GroupPicUpRequest
            {
                GroupUin = input.TargetGroupUin,
                SrcUin = keystore.Uin,
                FileId = 1,
                FileMd5 = input.FileMd5.UnHex(),
                FileSize = input.FileSize,
                FileName = input.FileMd5 + imageExt,
                SrcTerm = 2,
                PlatformType = 8,
                BuType = 212,
                PicWidth = (uint)size.X,
                PicHeight = (uint)size.Y,
                PicType = 1001,
                BuildVer = "1.0.0",
                OriginalPic = 1,
                SrvUpload = 0
            },
        };
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ImageGroupUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<GroupPicUp<GroupPicUpResponse>>(input.AsSpan());
        
        output = ImageGroupUploadEvent.Result((int)(packet.Body?.Result ?? 1),
                                              packet.Body?.UpUkey?.Hex(true) ?? "",
                                              packet.Body?.FileExit ?? false,
                                              (uint)(packet.Body?.Fileid ?? 0));
        
        extraEvents = null;
        return true;
    }
}