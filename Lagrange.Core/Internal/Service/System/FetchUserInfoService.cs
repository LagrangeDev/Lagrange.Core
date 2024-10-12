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
using static Lagrange.Core.Common.Entity.BotUserInfo;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchUserInfoEvent))]
[Service("OidbSvcTrpcTcp.0xfe1_2")]
internal class FetchUserInfoService : BaseService<FetchUserInfoEvent>
{
    private static readonly List<OidbSvcTrpcTcp0xFE1_2Key> _keys = new() {
        new() { Key = 20002 }, new() { Key = 27394 }, new() { Key = 20009 }, new() { Key = 20031 }, new() { Key = 101 }, new() { Key = 103 }, new() { Key = 102 }, new() { Key = 20022 }, new() { Key = 20023 }, new() { Key = 20024 }, new() { Key = 24002 }, new() { Key = 27037 }, new() { Key = 27049 }, new() { Key = 20011 }, new() { Key = 20016 }, new() { Key = 20021 }, new() { Key = 20003 }, new() { Key = 20004 }, new() { Key = 20005 }, new() { Key = 20006 }, new() { Key = 20020 }, new() { Key = 20026 }, new() { Key = 24007 }, new() { Key = 104 }, new() { Key = 105 }, new() { Key = 42432 }, new() { Key = 42362 }, new() { Key = 41756 }, new() { Key = 41757 }, new() { Key = 42257 }, new() { Key = 27372 }, new() { Key = 42315 }, new() { Key = 107 }, new() { Key = 45160 }, new() { Key = 45161 }, new() { Key = 27406 }, new() { Key = 62026 }, new() { Key = 20037 }
    };

    protected override bool Build(FetchUserInfoEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        // 27406 自定义状态文本
        // 27372 状态

        output = input.Uid == null
            ? new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2Uin>(new OidbSvcTrpcTcp0xFE1_2Uin
            {
                Uin = input.Uin,
                Field2 = 0,
                Keys = _keys,
            }, 0xfe1, 2, false, true).Serialize()
            : new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2Uid>(new OidbSvcTrpcTcp0xFE1_2Uid
            {
                Uid = input.Uid,
                Field2 = 0,
                Keys = _keys
            }, 0xfe1, 2).Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchUserInfoEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2Response>>(input);

        var bytesProperties = GetStringProperties(payload.Body.Body.Properties);
        var numberProperties = GetNumberProperties(payload.Body.Body.Properties);

        var birthday = GetBirthday(Encoding.UTF8.GetString(bytesProperties[20031]));
        var reg = DateTime.UnixEpoch.AddSeconds(numberProperties[20026]);

        string? qid = Encoding.UTF8.GetString(bytesProperties[27394]);

        uint statusId = numberProperties[27372];
        uint mask = 268435455 - statusId;
        mask = (uint)((int)mask >> 31);
        statusId -= 268435456 & mask;

        CustomStatus? customs = null;
        if (bytesProperties[27406].Length != 0)
        {
            using var stream = new MemoryStream();
            customs = Serializer.Deserialize<CustomStatus>(stream);
        }

        Avatar avatars;
        using (var stream = new MemoryStream(bytesProperties[101]))
        {
            avatars = Serializer.Deserialize<Avatar>(stream);
        }
        string? avatarurl = avatars.Url + "0";

        string? nickname = Encoding.UTF8.GetString(bytesProperties[20002]);
        string? city = Encoding.UTF8.GetString(bytesProperties[20020]);
        string? country = Encoding.UTF8.GetString(bytesProperties[20003]);
        string? school = Encoding.UTF8.GetString(bytesProperties[20021]);
        string? sign = Encoding.UTF8.GetString(bytesProperties[102]);

        var info = new BotUserInfo(payload.Body.Body.Uin, nickname, avatarurl, birthday, city, country, school, numberProperties[20037], reg, (GenderInfo)numberProperties[20009], qid, numberProperties[105], sign, new() { StatusId = statusId, FaceId = customs?.FaceId ?? 0, Msg = customs?.Msg });

        output = FetchUserInfoEvent.Result(0, info);
        extraEvents = null;
        return true;
    }

    private static DateTime GetBirthday(string birthday)
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
        foreach (var property in properties.BytesProperties)
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