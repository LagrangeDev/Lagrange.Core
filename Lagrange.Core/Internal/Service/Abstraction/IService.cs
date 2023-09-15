using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Service.Abstraction;

internal interface IService
{
    public bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out ProtocolEvent? output, out List<ProtocolEvent>? extraEvents);
    
    public bool Build(ProtocolEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket? output, out List<BinaryPacket>? extraPackets);
}