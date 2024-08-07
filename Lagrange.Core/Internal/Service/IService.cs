using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;

namespace Lagrange.Core.Internal.Service;

internal interface IService
{
    public bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ProtocolEvent? output, out List<ProtocolEvent>? extraEvents);
    
    public bool Build(ProtocolEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets);
}