using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Core.Service.Abstraction;

internal class BaseService<TEvent> : IService where TEvent : ProtocolEvent
{
    protected virtual bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out TEvent output, out List<ProtocolEvent>? extraEvents)
    {
        extraEvents = null;
        return (output = null!) != null;
    }

    protected virtual bool Build(TEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        extraPackets = null;
        return (output = null!) != null;
    }

    bool IService.Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out ProtocolEvent output, out List<ProtocolEvent>? extraEvents)
    {
        bool result = Parse(input, keystore, appInfo, device, out var @event, out extraEvents);
        output = @event;
        return result;
    }

    bool IService.Build(ProtocolEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets) =>
        Build((TEvent) input, keystore, appInfo, device, out output, out extraPackets);
}