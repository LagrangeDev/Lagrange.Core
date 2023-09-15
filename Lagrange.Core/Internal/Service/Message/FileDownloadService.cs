using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Message;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(FileDownloadEvent))]
[Service("OidbSvcTrpcTcp.0xe37_1200")]
internal class FileDownloadService : BaseService<FileDownloadEvent>
{
    protected override bool Build(FileDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.FileUuid == null || input.FileHash == null) throw new ArgumentNullException();
        if (input.SenderUid == null || input.ReceiverUid == null) throw new ArgumentNullException();

        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xE37_1200>(new OidbSvcTrpcTcp0xE37_1200
        {
            Body = new OidbSvcTrpcTcp0xE37_1200Body
            {
                ReceiverUid = input.ReceiverUid,
                FileUuid = input.FileUuid,
                FileHash = input.FileHash,
                T2 = 0
            },
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        extraPackets = null;
        
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FileDownloadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0xE37_1200Response>>(payload.AsSpan());
        
        var urlBuilder = new StringBuilder()
            .Append("http://")
            .Append(packet.Body.Body.Result.Server).Append(':').Append(packet.Body.Body.Result.Port)
            .Append(packet.Body.Body.Result.Url).Append("&isthumb=0");

        output = FileDownloadEvent.Result((int)packet.ErrorCode, urlBuilder.ToString());
        extraEvents = null;
        return true;
    }
}