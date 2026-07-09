using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchCookiesEventReq(List<string> domain) : ProtocolEvent
{
    public List<string> Domain { get; } = domain;
}

internal class FetchCookiesEventResp(Dictionary<string, string> cookies) : ProtocolEvent
{
    public Dictionary<string, string> Cookies { get; } = cookies;
}