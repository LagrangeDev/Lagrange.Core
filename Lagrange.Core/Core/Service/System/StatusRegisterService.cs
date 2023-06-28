using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.System;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(StatusRegisterEvent))]
[Service("trpc.qq_new_tech.status_svc.StatusService.Register")]
internal class StatusRegisterService : BaseService<StatusRegisterEvent>
{
    protected override bool Build(StatusRegisterEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new ServiceRegister
        {
            Guid = device.Guid.ToByteArray().Hex(true),
            Type = 0,
            CurrentVersion = appInfo.CurrentVersion,
            Field4 = 0,
            Field5 = 2052,
            Online = new OnlineOsInfo
            {
                User = device.DeviceName,
                Os = appInfo.Os,
                OsVer = device.SystemKernel,
                Field4 = "",
                OsLower = appInfo.Os.ToLower(),
            },
            Field7 = 0,
            Field8 = 0,
            Field9 = 1,
        };
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);

        output = new BinaryPacket(stream.ToArray());
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out StatusRegisterEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var response = Serializer.Deserialize<ServiceRegisterResponse>(payload.AsSpan());

        output = StatusRegisterEvent.Result(response.Message ?? "OK");
        extraEvents = null;
        return true;
    }
}