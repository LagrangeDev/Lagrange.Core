using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(GetC2cMessageEvent))]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoGetC2cMsg")]
internal class GetC2cMessageService : BaseService<GetC2cMessageEvent>
{
    protected override bool Build(GetC2cMessageEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new SsoGetC2cMsg
        {
            FriendUid = input.FriendUid,
            StartSequence = input.StartSequence,
            EndSequence = input.EndSequence
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GetC2cMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<SsoGetC2cMsgResponse>(input);
        var chains = payload.Messages?.Select(x => MessagePacker.Parse(x)).ToList() ?? new();

        output = GetC2cMessageEvent.Result((int)payload.Retcode, chains);
        extraEvents = null;
        return true;
    }
}