using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Service;

internal interface IService
{
    public bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ProtocolEvent? output, out List<ProtocolEvent>? extraEvents);
    
    public bool Build(ProtocolEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket? output, out List<BinaryPacket>? extraPackets);
}