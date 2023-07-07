using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Message;

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
        Console.WriteLine(input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix).Hex());
        return base.Parse(input, keystore, appInfo, device, out output, out extraEvents);
    }
}