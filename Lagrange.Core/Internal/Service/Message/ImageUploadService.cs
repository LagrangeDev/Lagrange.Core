using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(ImageUploadEvent))]
[Service("LongConn.OffPicUp")]
internal class ImageUploadService : BaseService<ImageUploadEvent>
{
    protected override bool Build(ImageUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
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
        
        var packet = new OffPicUp<OffPicUpRequest>
        {
            SubCmd = 1,
            Info = new OffPicUpRequest
            {
                SrcUin = keystore.Uin,
                FileId = 1,
                FileMd5 = input.FileMd5.UnHex(),
                FileSize = input.FileSize,
                FileName = input.FileMd5 + imageExt,
                SrcTerm = 2,
                PlatformType = 8,
                AddressBook = false,
                BuType = 8,
                PicOriginal = true,
                PicWidth = (uint)size.X,
                PicHeight = (uint)size.Y,
                PicType = 1001,
                SrvUpload = 0,
                TargetUid = input.TargetUid
            },
            NetType = 10
        };
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ImageUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OffPicUp<OffPicUpResponse>>(input.AsSpan());
        
        output = ImageUploadEvent.Result((int)(packet.Info?.Result ?? 1),
                                         packet.Info?.UpUkey?.Hex(true) ?? "", 
                                         packet.Info?.FileExit ?? false, 
                                         packet.Info?.UpResid ?? "");
        extraEvents = null;
        return true;
    }
}