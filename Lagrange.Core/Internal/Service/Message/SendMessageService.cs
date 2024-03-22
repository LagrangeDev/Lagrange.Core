using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(SendMessageEvent))]
[Service("MessageSvc.PbSendMsg")]
internal class SendMessageService : BaseService<SendMessageEvent>
{
    protected override bool Build(SendMessageEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = MessagePacker.Build(input.Chain, keystore.Uid ?? throw new Exception("No UID found in keystore"));

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SendMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var response = Serializer.Deserialize<SendMessageResponse>(input);
        var result = new MessageResult
        {
            Result = (uint)response.Result,
            Sequence = response.GroupSequence ?? response.PrivateSequence,
            Timestamp = response.Timestamp1,
        };
        
        output = SendMessageEvent.Result(response.Result, result);
        extraEvents = null;
        return true;
    }
}