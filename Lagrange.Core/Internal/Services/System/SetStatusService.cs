using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<SetStatusEventReq>(Protocols.All)]
[Service("trpc.qq_new_tech.status_svc.StatusService.SetStatus")]
internal class SetStatusService : BaseService<SetStatusEventReq, SetStatusEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(SetStatusEventReq input, BotContext context)
    {
        var packet = new SetStatusReq
        {
            Status = input.Status,
            ExtStatus = input.ExtStatus,
            BatteryStatus = input.BatteryStatus,
            CustomExt = input.CustomExt is { } customExt
                ? new SetStatusCustomExt
                {
                    FaceId = customExt.FaceId,
                    Wording = customExt.Wording,
                    FaceType = customExt.FaceType
                }
                : null
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(packet));
    }

    protected override ValueTask<SetStatusEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (input.IsEmpty) return ValueTask.FromResult(new SetStatusEventResp(0, string.Empty));

        var packet = ProtoHelper.Deserialize<SetStatusResp>(input.Span);
        return ValueTask.FromResult(new SetStatusEventResp(packet.ErrCode, packet.ErrMsg));
    }
}
