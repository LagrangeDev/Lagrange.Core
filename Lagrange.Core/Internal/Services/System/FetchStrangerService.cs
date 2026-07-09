using System.Buffers.Binary;
using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchStrangerByUinEventReq>(Protocols.All)]
[EventSubscribe<FetchStrangerByUidEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xfe1_2")]
internal class FetchStrangerService : BaseService<FetchStrangerEventReqBase, FetchStrangerEventResp>
{
    private static readonly uint Command = 0xfe1;

    private static readonly uint Service = 2;

    private static string Tag => $"OidbSvcTrpcTcp.0x{Command:X}_{Service}";

    public static readonly List<FetchStrangerRequestKey> Keys = [
        new() { Key = 101 },      // Avatar; Object
        new() { Key = 102 },      // Sign
        new() { Key = 103 },      // Remark
        new() { Key = 104 },      // Tag; Object
        new() { Key = 105 },      // Level
        new() { Key = 107 },      // Business List; Object
        new() { Key = 20002 },    // Nickname
        new() { Key = 20003 },    // Country
        new() { Key = 20004 },    // City
        new() { Key = 20006 },    // Home City
        new() { Key = 20009 },    // Gender; 1 Male 2 Female 255 Unknown
        new() { Key = 20011 },    // EMail
        new() { Key = 20016 },    // Desensitized mobile phone number
        new() { Key = 20020 },    // Municipal district
        new() { Key = 20021 },    // School
        new() { Key = 20026 },    // Registration Time; Only year, hour, minute, second
        new() { Key = 20031 },    // Birthday; 
        new() { Key = 20037 },    // Age
        new() { Key = 27394 },    // QID
    ];

    protected override ValueTask<ReadOnlyMemory<byte>> Build(FetchStrangerEventReqBase input, BotContext context)
    {
        return ValueTask.FromResult(input switch
        {
            FetchStrangerByUinEventReq req => ProtoHelper.Serialize(new Oidb
            {
                Command = Command,
                Service = Service,
                Body = ProtoHelper.Serialize(new FetchStrangerByUinRequest
                {
                    Uin = req.Uin,
                    Keys = Keys,
                }),
                Reserved = 1
            }),
            FetchStrangerByUidEventReq req => ProtoHelper.Serialize(new Oidb
            {
                Command = Command,
                Service = Service,
                Body = ProtoHelper.Serialize(new FetchStrangerByUidRequest
                {
                    Uid = req.Uid,
                    Keys = Keys,
                })
            }),
            _ => throw new NotSupportedException(),
        });
    }

    protected override ValueTask<FetchStrangerEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var oidb = ProtoHelper.Deserialize<Oidb>(input.Span);
        if (oidb.Result != 0)
        {
            context.LogWarning(Tag, "Error: {0}, Message: {1}", null, oidb.Result, oidb.Message);
            throw new OperationException((int)oidb.Result, oidb.Message);
        }
        var response = ProtoHelper.Deserialize<FetchStrangerResponse>(oidb.Body.Span);

        var numbers = response.Body.Properties.NumberProperties.ToDictionary(
            property => property.Key,
            property => property.Value
        );
        var bytes = response.Body.Properties.BytesProperties.ToDictionary(
            property => property.Key,
            property => property.Value
        );

        // Check exists
        if (!bytes.TryGetValue(20002, out byte[]? nicknameBytes))
        {
            throw new OperationException(-1, "Stranger not found");
        }

        // Birthday
        byte[] birthday = bytes[20031];
        int year = BinaryPrimitives.ReadUInt16BigEndian(birthday.AsSpan(0, 2));
        int month = birthday[2];
        int day = birthday[3];

        return ValueTask.FromResult(new FetchStrangerEventResp(new BotStranger(
            response.Body.Uin,
            Encoding.UTF8.GetString(nicknameBytes),
            string.Empty, // Can't not get uid
            Encoding.UTF8.GetString(bytes[102]),
            Encoding.UTF8.GetString(bytes[103]),
            numbers[105],
            (BotGender)numbers[20009],
            DateTimeOffset.FromUnixTimeSeconds((long)numbers[20026]).DateTime,
            month != 0 && day != 0 ? new DateTime(year != 0 ? year : 1, month, day) : null,
            numbers[20037],
            Encoding.UTF8.GetString(bytes[27394]),
            Encoding.UTF8.GetString(bytes[20003]),
            Encoding.UTF8.GetString(bytes[20004]),
            bytes.TryGetValue(200021, out byte[]? value) ? Encoding.UTF8.GetString(value) : null
        )));
    }
}