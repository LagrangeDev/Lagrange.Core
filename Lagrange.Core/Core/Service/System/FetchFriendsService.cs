using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Generics;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Packets.Service.Oidb.Resopnse;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(FetchFriendsEvent))]
[Service("OidbSvcTrpcTcp.0xfd4_1")]
internal class FetchFriendsService : BaseService<FetchFriendsEvent>
{
    protected override bool Build(FetchFriendsEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFD4_1>(new OidbSvcTrpcTcp0xFD4_1
        {
            Body = new List<OidbSvcTrpcTcp0xFD4_1Body>
            {
                new() { Type = 1, Number = new OidbNumber { Numbers = { 103, 102, 20002 } } },
                new() { Type = 4, Number = new OidbNumber { Numbers = { 100, 101, 102 } } }
            }
        });
        
        /*
         * OidbNumber里面的东西代表你想要拿到的Property，这些Property将会在返回的数据里面的Preserve的Field，
         * 102：个性签名
         * 103：备注
         * 20002：昵称
         */
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);

        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchFriendsEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0xFD4_1Response>>(payload.AsSpan());

        var friends = new List<BotFriend>();
        foreach (var raw in packet.Body.Friends)
        {
            var properties = Property(raw.Additional.Layer1.Properties);
            friends.Add(new BotFriend(raw.Uin, raw.Uid, properties[20002], properties[103], properties[102]));
        }

        output = FetchFriendsEvent.Result(0, friends);
        extraEvents = null;
        return true;
    }

    private static Dictionary<uint, string> Property(List<OidbFriendProperty> properties)
    {
        var dict = new Dictionary<uint, string>(properties.Capacity);
        foreach (var property in properties) dict[property.Code] = property.Value;
        return dict;
    }
}