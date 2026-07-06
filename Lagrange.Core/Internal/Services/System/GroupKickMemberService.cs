using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<GroupKickMemberEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x8a0_1")]
internal class GroupKickMemberService : BaseService<GroupKickMemberEventReq, GroupKickMemberEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(GroupKickMemberEventReq input, BotContext context)
    {
        var request = new D8A0_1ReqBody
        {
            GroupUin = (ulong)input.GroupUin,
            TargetUid = input.TargetUid,
            RejectAddRequest = input.RejectAddRequest,
            Reason = input.Reason
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(new Oidb
        {
            Command = 0x8a0,
            Service = 1,
            Body = ProtoHelper.Serialize(request)
        }));
    }

    protected override ValueTask<GroupKickMemberEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var oidb = ProtoHelper.Deserialize<Oidb>(input.Span);
        var response = oidb.Body.Length == 0
            ? new D8A0_1RspBody()
            : ProtoHelper.Deserialize<D8A0_1RspBody>(oidb.Body.Span);

        return ValueTask.FromResult(new GroupKickMemberEventResp((int)oidb.Result, response.ErrorMsg ?? oidb.Message));
    }
}
