using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class HighwaySessionEventReq : ProtocolEvent;

internal class HighwaySessionEventResp(Dictionary<uint, List<string>> highwayUrls, byte[] sigSession) : ProtocolEvent
{
    public Dictionary<uint, List<string>> HighwayUrls { get; } = highwayUrls;

    public byte[] SigSession { get; } = sigSession;
}