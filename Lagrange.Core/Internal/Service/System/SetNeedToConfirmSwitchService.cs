using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(SetNeedToConfirmSwitchEvent))]
[Service("OidbSvcTrpcTcp.0x1277_0")]
internal class SetNeedToConfirmSwitchService : BaseService<SetNeedToConfirmSwitchEvent>
{
    protected override bool Build(SetNeedToConfirmSwitchEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x1277_0>(new OidbSvcTrpcTcp0x1277_0
        {
            Body = new OidbSvcTrpcTcp0x1277_0Body
            {
                Device = new OidbSvcTrpcTcp0x1277_0Device
                {
                    Guid = device.Guid.ToByteArray(),
                    AppId = (uint)appInfo.AppId,
                    PackageName = appInfo.PackageName
                },
                GuidEncryptedType = false,
                AutoLogin = input.EnableNoNeed
            }
        }, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SetNeedToConfirmSwitchEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = SetNeedToConfirmSwitchEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}