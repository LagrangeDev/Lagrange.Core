using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupClockInEvent))]
[Service("OidbSvcTrpcTcp.0xeb7_1")]
internal class GroupClockInService : BaseService<GroupClockInEvent>
{
    protected override bool Build(GroupClockInEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xEB7_1>(new()
        {
            Body = new()
            {
                Uin = keystore.Uin.ToString(),
                GroupUin = input.GroupUin.ToString(),
                AppVersion = appInfo.CurrentVersion
            }
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out GroupClockInEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xEB7_1Response>>(input);
        extraEvents = null;

        var payloadResult = payload.Body?.Body?.Result;
        if (payload.ErrorCode != 0 || payloadResult == null)
        {
            output = GroupClockInEvent.Result((int)payload.ErrorCode, new BotGroupClockInResult(false));
            return true;
        }

        if (payloadResult.ClockInInfo1 is not { Length: > 1 } || !long.TryParse(payloadResult.ClockInInfo1[1], out var clockInTimestamp))
            clockInTimestamp = 0;

        var result = new BotGroupClockInResult(true)
        {
            Title = payloadResult.Title ?? string.Empty,
            KeepDayText = payloadResult.KeepDayText ?? string.Empty,
            GroupRankText = payloadResult.ClockInInfo1 is { Length: > 0 } ? payloadResult.ClockInInfo1[0] : string.Empty,
            ClockInUtcTime = DateTime.UnixEpoch + TimeSpan.FromSeconds(clockInTimestamp),
            DetailUrl = payloadResult.DetailUrl ?? string.Empty,
        };
        output = GroupClockInEvent.Result((int)payload.ErrorCode, result);

        return true;
    }
}
