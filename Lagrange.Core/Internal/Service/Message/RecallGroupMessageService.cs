using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Message;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(RecallGroupMessageEvent))]
[Service("trpc.msg.msg_svc.MsgService.SsoGroupRecallMsg")]
internal class RecallGroupMessageService : BaseService<RecallGroupMessageEvent>
{
    protected override bool Build(RecallGroupMessageEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new GroupRecallMsg
        {
            Type = 1,
            GroupUin = input.GroupUin,
            Field3 = new GroupRecallMsgField3
            {
                Sequence = input.Sequence,
                Field3 = 0
            },
            Field4 = new GroupRecallMsgField4 { Field1 = 0 }
        };
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out RecallGroupMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        Console.WriteLine("Recall: " + payload.Hex());
        
        output = RecallGroupMessageEvent.Result(0);
        extraEvents = null;
        return true;
    }
}