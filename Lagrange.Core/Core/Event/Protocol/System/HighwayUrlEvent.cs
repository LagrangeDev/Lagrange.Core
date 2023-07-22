namespace Lagrange.Core.Core.Event.Protocol.System;

#pragma warning disable CS8618

internal class HighwayUrlEvent : ProtocolEvent
{
    private readonly Dictionary<uint, List<Uri>> _highwayUrls;
    
    private HighwayUrlEvent() : base(true) { }
    
    private HighwayUrlEvent(int resultCode, Dictionary<uint, List<Uri>> highwayUrls) : base(resultCode)
    {
        _highwayUrls = highwayUrls;
    }
    
    public static HighwayUrlEvent Create() => new();
    
    public static HighwayUrlEvent Result(int resultCode, Dictionary<uint, List<Uri>> highwayUrls)
    {
        return new HighwayUrlEvent(resultCode, highwayUrls);
    }
}