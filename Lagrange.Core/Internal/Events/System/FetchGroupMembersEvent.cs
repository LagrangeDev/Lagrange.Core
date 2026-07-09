using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchGroupMembersEventReq(long groupUin, byte[]? cookie) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public byte[]? Cookie { get; } = cookie;
}

internal class FetchGroupMembersEventResp(List<BotGroupMember> groupMembers, byte[]? cookie) : ProtocolEvent
{
    public List<BotGroupMember> GroupMembers { get; } = groupMembers;

    public byte[]? Cookie { get; } = cookie;
}