namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class HighwayUrlEvent : ProtocolEvent
{
    public Dictionary<uint, List<Uri>> HighwayUrls { get; }

    public byte[] SigSession { get; }

    private HighwayUrlEvent() : base(true) { }

    private HighwayUrlEvent(int resultCode, byte[] sigSession, Dictionary<uint, List<Uri>> highwayUrls) : base(resultCode)
    {
        SigSession = sigSession;
        HighwayUrls = highwayUrls;
    }

    public static HighwayUrlEvent Create() => new();

    public static HighwayUrlEvent Result(int resultCode, byte[] sigSession, Dictionary<uint, List<Uri>> highwayUrls) => 
            new(resultCode, sigSession, highwayUrls);
}