using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Message;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Message;

[EventSubscribe(typeof(PushMessageEvent))]
[Service("trpc.msg.olpush.OlPushService.MsgPush")]
internal class PushMessageService : BaseService<PushMessageEvent>
{
    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out PushMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var message = Serializer.Deserialize<PushMsg>(payload.AsSpan());
        try
        {
            var chain = MessagePacker.Parse(message.Message);
            var chainBuilder = new StringBuilder();

            chainBuilder.Append("[MessageChain]: ");
            foreach (var entity in chain)
            {
                chainBuilder.Append(entity.ToPreviewString());
                if (chain.Last() != entity) chainBuilder.Append(" | ");
            }
            
            Console.WriteLine(chainBuilder.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.WriteLine(payload.Hex());
        
        output = PushMessageEvent.Create();
        extraEvents = null;
        return true;
    }
}