namespace Lagrange.Core.Internal.Event.Action;

internal class FetchCustomFaceEvent : ProtocolEvent
{
    public List<string> Urls { get; } = new();
    
    private FetchCustomFaceEvent() : base(true) { }

    private FetchCustomFaceEvent(int resultCode, List<string> urls) : base(resultCode)
    {
        Urls = urls;
    }

    public static FetchCustomFaceEvent Create() => new();
    
    public static FetchCustomFaceEvent Result(int resultCode, List<string> urls) => new(resultCode, urls);
}