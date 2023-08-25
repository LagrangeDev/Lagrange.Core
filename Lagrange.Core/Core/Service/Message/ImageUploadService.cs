using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Generics;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Packets.Service.Oidb.Response;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Message;

[EventSubscribe(typeof(ImageUploadEvent))]
[Service("OidbSvcTrpcTcp.0x11c5_100")]
internal class ImageUploadService : BaseService<ImageUploadEvent>
{
    protected override bool Build(ImageUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var buffer = new byte[1024]; // parse image header
        int bytes = input.Stream.Read(buffer.AsSpan());
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
        
        var body = new OidbSvcTrpcTcp0x11C5_100
        {
            Body1 = new OidbSvcTrpcTcp0x11C5_100Body1
            {
                Command = new OidbTwoNumber { Number1 = 1, Number2 = 100 },
                Field2 = new OidbSvcTrpcTcp0x11C5_100Body1Field2
                {
                    Field101 = 2,
                    Field102 = 1,
                    Field200 = 1,
                    Field201 = new OidbSvcTrpcTcp0x11C5_100Body1Field2Field201 { Type = 2, TargetUid = input.TargetUid }
                },
                Field3 = new OidbSvcTrpcTcp0x11C5_100Body1Field3 { Type = 2 }
            },
            Body2 = new OidbSvcTrpcTcp0x11C5_100Body2
            {
                Field1 = new OidbSvcTrpcTcp0x11C5_100Body2Field1
                {
                    Field1 = new OidbSvcTrpcTcp0x11C5_100Body2Field1Field1
                    {
                        FileSize = input.FileSize,
                        FileMd5 = input.FileMd5,
                        FileSha1 = input.FileSha1,
                        FileName = input.FileMd5 + imageExt,
                        Field5 = new OidbSvcTrpcTcp0x11C5_100Body2Field1Field1Field5 { Type = 1, Field2 = 1001, Field3 = 0, Field4 = 0 },
                        ImageWidth = (uint)size.X,
                        ImageHeight = (uint)size.Y,
                        Field8 = 0,
                        Field9 = 1
                    },
                    Field2 = 0
                },
                Field2 = 1,
                Field3 = 0,
                Random = (uint)Random.Shared.Next(),
                Field5 = 0,
                Field6 = new OidbSvcTrpcTcp0x11C5_100Body2Field1Field6
                {
                    Field1 = new OidbSvcTrpcTcp0x11C5_100Body2Field1Field6Field1 { Field1 = 0, Field2 = "" },
                    Field3 = new OidbSvcTrpcTcp0x11C5_100Body2Field1Field6Field3 { Field3 = 0, Field4 = 0, Field5 = "" }
                },
                Sequence = (uint)Random.Shared.Next()
            }
        };
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x11C5_100>(body);
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out ImageUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x11C5_100Response>>(payload.AsSpan());
        
        output = ImageUploadEvent.Result(0, packet.Body.Data.HighwayTicket, packet.Body.Data.CommonAdditional, packet.Body.Data.ImageInfo);
        extraEvents = null;
        return true;
    }
}