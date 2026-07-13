using System.Globalization;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Response;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupClockInEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xeb7_1")]
internal class GroupClockInService : OidbService<GroupClockInEventReq, GroupClockInEventResp, DEB7ReqBody, DEB7RspBody>
{
    protected override uint Command => 0xeb7;

    protected override uint Service => 1;

    protected override Task<DEB7ReqBody> ProcessRequest(GroupClockInEventReq request, BotContext context)
    {
        return Task.FromResult(new DEB7ReqBody
        {
            SignInWrite = new EB7SignInWriteReq
            {
                Uid = context.BotUin.ToString(CultureInfo.InvariantCulture),
                GroupId = request.GroupUin.ToString(CultureInfo.InvariantCulture),
                ClientVersion = context.AppInfo.CurrentVersion
            }
        });
    }

    protected override Task<GroupClockInEventResp> ProcessResponse(DEB7RspBody response, BotContext context)
    {
        var write = response.SignInWrite ?? throw new OperationException(-1, "Group clock-in result was not returned");
        if (write.Ret is { Code: not 0 } ret) throw new OperationException(ret.Code, ret.Msg);

        var doneInfo = write.DoneInfo ?? throw new OperationException(-1, "Group clock-in details were not returned");
        if (doneInfo.BelowPortraitWords is not { Count: > 1 } words || !long.TryParse(words[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out long clockInTimestamp))
        {
            clockInTimestamp = 0;
        }

        var result = new BotGroupClockInResult(true)
        {
            Title = doneInfo.LeftTitleWord ?? string.Empty,
            KeepDayText = doneInfo.RightDescWord ?? string.Empty,
            GroupRankText = doneInfo.BelowPortraitWords?.FirstOrDefault() ?? string.Empty,
            ClockInUtcTime = DateTime.UnixEpoch + TimeSpan.FromSeconds(clockInTimestamp),
            DetailUrl = doneInfo.RecordUrl ?? string.Empty
        };

        return Task.FromResult(new GroupClockInEventResp(result));
    }
}
