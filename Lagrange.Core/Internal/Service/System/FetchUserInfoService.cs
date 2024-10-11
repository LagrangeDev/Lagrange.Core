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
    protected override bool Build(FetchUserInfoEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var keys = new List<uint> { 20002, 27394, 20009, 20031, 101, 103, 102, 20022, 20023, 20024, 24002, 27037, 27049, 20011, 20016, 20021, 20003, 20004, 20005, 20006, 20020, 20026, 24007, 104, 105, 42432, 42362, 41756, 41757, 42257, 27372, 42315, 107, 45160, 45161, 27406, 62026, 20037 };

        // 27406 自定义状态文本
        // 27372 状态

        object packet = input.Uid == null
            ? new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2Uin>(new OidbSvcTrpcTcp0xFE1_2Uin
            {
                Uin = input.Uin,
                Field2 = 0,
                Keys = keys.Select(x => new OidbSvcTrpcTcp0xFE1_2Key { Key = x }).ToList()
            }, 0xfe1, 2, false, true)
            : new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2>(new OidbSvcTrpcTcp0xFE1_2
            {
                Uid = input.Uid,
                Field2 = 0,
                Keys = keys.Select(x => new OidbSvcTrpcTcp0xFE1_2Key { Key = x }).ToList()
            }, 0xfe1, 2);

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

        //如果是嵌套proto就Serializer.Deserialize(Value)如果是String就Encoding.UTF8.GetString(Value)

        var birthday = GetBirthday(Encoding.UTF8.GetString(str[20031]));
        var reg = DateTime.UnixEpoch.AddSeconds(num[20026]);

        string? qid = Encoding.UTF8.GetString(str[27394]);

        byte[] custom = str[27406];

        CustomStatus customs;
        using (var stream = new MemoryStream(custom))
        {
            customs = Serializer.Deserialize<CustomStatus>(stream);
        }
        object[] customarray = new object[] { customs.FaceId, customs.Msg ?? "" };

        byte[] avatar = str[101];
        Avatar avatars;
        using (var stream = new MemoryStream(avatar))
        {
            avatars = Serializer.Deserialize<Avatar>(stream);
        }
        string? avatarurl = avatars.Url + "640";


        string? nickname = Encoding.UTF8.GetString(str[20002]);
        string? city = Encoding.UTF8.GetString(str[20020]);
        string? country = Encoding.UTF8.GetString(str[20003]);
        string? school = Encoding.UTF8.GetString(str[20021]);
        string? sign = Encoding.UTF8.GetString(str[102]);



        var info = new BotUserInfo(payload.Body.Body.Uin, nickname, avatarurl, birthday, city, country, school, num[20037], reg, num[20009], qid, num[105], sign, num[27372], customarray);

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
        if (year != 0 && month is >= 1 and <= 12 && day >= 1 && day <= DateTime.DaysInMonth(year, month))
        {
            return new DateTime(year, month, day);
        }
        return new DateTime(1970, 1, 1);
    }

    private static Dictionary<uint, byte[]> GetStringProperties(OidbSvcTrpcTcp0xFE1_2ResponseProperty properties)
    {
        var result = new Dictionary<uint, byte[]>();
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

    [ProtoContract]
    public class CustomStatus
    {
        [ProtoMember(1)] public uint FaceId { get; set; }

        [ProtoMember(2)] public string? Msg { get; set; }
    }

    [ProtoContract]
    public class Avatar
    {
        [ProtoMember(5)] public string? Url { get; set; }
    }

}