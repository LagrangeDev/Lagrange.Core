using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(GetGroupMessageEvent))]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoGetGroupMsg")]
internal class GetGroupMessageService : BaseService<GetGroupMessageEvent>
{
    protected override bool Build(GetGroupMessageEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new SsoGetGroupMsg
        {
            Info = new SsoGetGroupMsgInfo
            {
                GroupUin = input.GroupUin,
                StartSequence = input.StartSequence,
                EndSequence = input.EndSequence
            },
            Direction = true
        };
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GetGroupMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<SsoGetGroupMsgResponse>(input);
        var chains = payload.Body.Messages.Select(x => MessagePacker.Parse(x)).ToList();
        
        output = GetGroupMessageEvent.Result(0, chains);
        extraEvents = null;
        return true;
    }
}