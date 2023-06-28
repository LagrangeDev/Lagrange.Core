using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Core.Service.Message;

[EventSubscribe(typeof(SendMessageEvent))]
[Service("MessageSvc.PbSendMsg")]
internal class SendMessageService : BaseService<SendMessageEvent>
{
    protected override bool Build(SendMessageEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        return base.Build(input, keystore, appInfo, device, out output, out extraPackets);
    }
}