using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Action;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf.Meta;

namespace Lagrange.Core.Core.Service.Action;

[EventSubscribe(typeof(RequestFriendEvent))]
[Service("OidbSvcTrpcTcp.0x7c2_5")]
internal class RequestFriendService : BaseService<RequestFriendEvent>
{
    private static readonly RuntimeTypeModel Serializer;
    
    static RequestFriendService()
    {
        Serializer = RuntimeTypeModel.Create();
        Serializer.UseImplicitZeroDefaults = false;
    }
    
    protected override bool Build(RequestFriendEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x7C2_5>(new OidbSvcTrpcTcp0x7C2_5
        {
            SelfUin = keystore.Uin,
            TargetUin = input.TargetUin,
            Field3 = 1,
            Field4 = 1,
            Field5 = 0,
            Remark = "",
            SourceId = 1,
            SubSourceId = 3,
            Verify = input.Message,
            CategoryId = 0,
            Answer = input.Question,
            Field28 = 1,
            Field29 = 1
        });

        var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        Console.WriteLine(output.ToArray().Hex());
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out RequestFriendEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        
        Console.WriteLine(payload.Hex());
        
        output = RequestFriendEvent.Result(0);
        extraEvents = null;
        return true;
    }
}