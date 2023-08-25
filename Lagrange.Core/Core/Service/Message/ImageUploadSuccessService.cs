using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Generics;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Message;

[EventSubscribe(typeof(ImageUploadSuccessEvent))]
[Service("OidbSvcTrpcTcp.0x11c5_101")]
internal class ImageUploadSuccessService : BaseService<ImageUploadSuccessEvent>
{
    protected override bool Build(ImageUploadSuccessEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x11C5_101>(new OidbSvcTrpcTcp0x11C5_101
        {
            Body1 = new OidbSvcTrpcTcp0x11C5_101Body1
            {            
                Command = new OidbTwoNumber { Number1 = 2, Number2 = 101 },
                Field2 = new OidbSvcTrpcTcp0x11C5_100Body1Field2
                {
                    Field101 = 2,
                    Field102 = 1,
                    Field200 = 1,
                    Field201 = new OidbSvcTrpcTcp0x11C5_100Body1Field2Field201 { Type = 2, TargetUid = input.TargetUid }
                },
                Field3 = new OidbSvcTrpcTcp0x11C5_100Body1Field3 { Type = 2 }
            },
            CommandAdditional = input.CommonAdditional
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out ImageUploadSuccessEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        Console.WriteLine("ImageUploadSuccessService: " + payload.Hex());

        output = ImageUploadSuccessEvent.Result(0);
        extraEvents = null;
        return true;
    }
}