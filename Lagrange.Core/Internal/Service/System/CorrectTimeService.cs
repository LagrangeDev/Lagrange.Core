using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(CorrectTimeEvent))]
[Service("Client.CorrectTime", 13)]
internal class CorrectTimeService : BaseService<CorrectTimeEvent>
{
    protected override bool Build(CorrectTimeEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        output = new BinaryPacket().WriteUint(4).ToArray();
        extraPackets = null;
        return true;
    }
}