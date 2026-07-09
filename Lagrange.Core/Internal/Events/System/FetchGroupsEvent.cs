using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchGroupsEventReq : ProtocolEvent;

internal class FetchGroupsEventResp(List<BotGroup> groups) : ProtocolEvent
{
    public List<BotGroup> Groups { get; } = groups;
}