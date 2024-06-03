using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchUserInfoEvent))]
[Service("OidbSvcTrpcTcp.0xfe1_2")]
internal class FetchUserInfoService : BaseService<FetchUserInfoEvent>
{
    protected override bool Build(FetchUserInfoEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var keys = new List<uint> { 20002, 27394, 20009, 20031, 101, 103, 102, 20022, 20023, 20024, 24002, 27037, 27049, 20011, 20016, 20021, 20003, 20004, 20005, 20006, 20020, 20026, 24007, 104, 105, 42432, 42362, 41756, 41757, 42257, 27372, 42315, 107, 45160, 45161, 27406, 62026, 20037 };

        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2>(new OidbSvcTrpcTcp0xFE1_2
        {
            Uid = input.Uid,
            Field2 = 0,
            Keys = keys.Select(x => new OidbSvcTrpcTcp0xFE1_2Key { Key = x }).ToList()
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchUserInfoEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2Response>>(input);
        
        var str = GetStringProperties(payload.Body.Body.Properties);
        var num = GetNumberProperties(payload.Body.Body.Properties);

        var birthday = GetBirthday(str[20031]);
        var reg = DateTime.UnixEpoch.AddSeconds(num[20026]);
        var info = new BotUserInfo(payload.Body.Body.Uin, payload.Body.Body.Uid, str[20002], birthday, str[20020], str[20003], str[20021], num[20037], reg, num[20009]);

        output = FetchUserInfoEvent.Result(0, info);
        extraEvents = null;
        return true;
    }

    private static DateTime GetBirthday(String birthday)
    {
        var bin = new BinaryPacket(Encoding.ASCII.GetBytes(birthday));
        var year = bin.ReadUshort();
        var month = bin.ReadByte();
        var day = bin.ReadByte();
        if (year != 0 && month >= 1 && month <= 12 && day >= 1 && day <= DateTime.DaysInMonth(year, month))
        {
            return new DateTime(year, month, day);
        }
        return new DateTime(1970, 1, 1);
    }
    
    private static Dictionary<uint, string> GetStringProperties(OidbSvcTrpcTcp0xFE1_2ResponseProperty properties)
    {
        var result = new Dictionary<uint, string>();
        foreach (var property in properties.StringProperties)
        {
            result[property.Code] = property.Value;
        }

        return result;
    }
    
    private static Dictionary<uint, uint> GetNumberProperties(OidbSvcTrpcTcp0xFE1_2ResponseProperty properties)
    {
        var result = new Dictionary<uint, uint>();
        foreach (var property in properties.NumberProperties)
        {
            result[property.Number1] = property.Number2;
        }

        return result;
    }
}