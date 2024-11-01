using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(ImageOcrEvent))]
[Service("OidbSvcTrpcTcp.0xe07_0")]
internal class ImageOcrService : BaseService<ImageOcrEvent>
{
    protected override bool Build(ImageOcrEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xE07_0>(new OidbSvcTrpcTcp0xE07_0
        {
            Version = 1,
            Client = 0,
            Entrance = 1,
            OcrReqBody = new OcrReqBody
            {
                ImageUrl = input.Url,
                OriginMd5 = "",
                AfterCompressMd5 = "",
                AfterCompressFileSize = "",
                AfterCompressWeight = "",
                AfterCompressHeight = "",
                IsCut = false
            }
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ImageOcrEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xE07_0_Response>>(input);
        var response = new ImageOcrResult(
            packet?.Body.OcrRspBody.TextDetections.Select(d => new Common.Entity.TextDetection(
                d.DetectedText,
                (int)d.Confidence,
                d.Polygon.Coordinates.Select(c => new Common.Entity.Coordinate(c.X, c.Y)).ToList() 
            )).ToList() ?? new List<Common.Entity.TextDetection>(), 
            packet?.Body.OcrRspBody.Language ?? string.Empty 
        );
        output = ImageOcrEvent.Result(packet?.Body.RetCode ?? -1, response);
        extraEvents = null;
        return true;
    }
}