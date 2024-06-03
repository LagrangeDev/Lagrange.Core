using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchFriendsEvent))]
[Service("OidbSvcTrpcTcp.0xfd4_1")]
internal class FetchFriendsService : BaseService<FetchFriendsEvent>
{
    private const int MaxFriendCount = 300;
    
    protected override bool Build(FetchFriendsEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFD4_1>(new OidbSvcTrpcTcp0xFD4_1
        {
            FriendCount = MaxFriendCount, // max value
            Body = new List<OidbSvcTrpcTcp0xFD4_1Body>
            {
                new() { Type = 1, Number = new OidbNumber { Numbers = { 103, 102, 20002 } } },
                new() { Type = 4, Number = new OidbNumber { Numbers = { 100, 101, 102 } } }
            }
        });

        if (input.NextUin != null) packet.Body.NextUin = new OidbSvcTrpcTcp0xFD4_1Uin { Uin = input.NextUin.Value };
        
        /*
         * OidbNumber里面的东西代表你想要拿到的Property，这些Property将会在返回的数据里面的Preserve的Field，
         * 102：个性签名
         * 103：备注
         * 20002：昵称
         */
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchFriendsEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFD4_1Response>>(input);

        var friends = new List<BotFriend>();
        foreach (var raw in packet.Body.Friends)
        {
            var additional = raw.Additional.First(x => x.Type == 1);
            var properties = Property(additional.Layer1.Properties);
            friends.Add(new BotFriend(raw.Uin, raw.Uid, properties[20002], properties[103], properties[102]));
        }
        
        output = FetchFriendsEvent.Result(0, friends, packet.Body.Next?.Uin); // 全家4完了才能想出来这种分页的逻辑
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