using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(FriendPokeEvent))]
[EventSubscribe(typeof(GroupPokeEvent))]
[Service("OidbSvcTrpcTcp.0xed3_1")]
internal class PokeService : BaseService<FriendPokeEvent>
{
    protected override bool Build(FriendPokeEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        switch (input)
        {
            case GroupPokeEvent group:
            {
                var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xED3_1>(new OidbSvcTrpcTcp0xED3_1
                {
                    Uin = group.FriendUin,
                    GroupUin = group.GroupUin,
                    Ext = 0
                });
                output = packet.Serialize();
                break;
            }
            case { } friend:
            {
                var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xED3_1>(new OidbSvcTrpcTcp0xED3_1
                {
                    Uin = friend.FriendUin,
                    FriendUin = friend.FriendUin,
                    Ext = 0
                });
                output = packet.Serialize();
                break;
            }
            default: throw new InvalidDataException();
        }

        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FriendPokeEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        
        output = FriendPokeEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}