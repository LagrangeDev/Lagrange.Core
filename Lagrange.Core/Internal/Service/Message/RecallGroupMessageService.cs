using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message;
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
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out RecallGroupMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = RecallGroupMessageEvent.Result(0);
        extraEvents = null;
        return true;
    }
}