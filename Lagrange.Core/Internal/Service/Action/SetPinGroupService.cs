using System.Buffers.Binary;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(SetPinGroupEvent))]
[Service("OidbSvcTrpcTcp.0x5d6_1")]
internal class SetPinGroupService : BaseService<SetPinGroupEvent>
{
    protected override bool Build(SetPinGroupEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x5D6_1>(
            new OidbSvcTrpcTcp0x5D6_1
            {
                Field1 = 0,
                Info = new OidbSvcTrpcTcp0x5D6_1Info
                {
                    GroupUin = input.Uin,
                    Field400 = new OidbSvcTrpcTcp0x5D6_1Field4_2_400
                    {
                        Field1 = 13569,
                        Timestamp = input.IsPin ? GetTimestamp() : Array.Empty<byte>()
                    }
                },
                Field3 = 11
            }
        );

        output = packet.Serialize();
        extraPackets = null;

        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out SetPinGroupEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = SetPinGroupEvent.Result((int)payload.ErrorCode, payload.ErrorMsg);
        extraEvents = null;
        return true;
    }

    private static byte[] GetTimestamp() {
        byte[] timestamp = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(timestamp, (int)DateTimeOffset.Now.ToUnixTimeSeconds());
        return timestamp;
    }
}