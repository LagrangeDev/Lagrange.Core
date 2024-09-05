namespace Lagrange.Core.Internal.Event.Action;

internal class ImageExpiresEvent : ProtocolEvent
{
    public string? Url { get; set; }


    private ImageExpiresEvent(string? url) : base(0)
    {
        Url = url;
    }

    private ImageExpiresEvent(int resultCode) : base(resultCode) { }
    
    public static ImageExpiresEvent Create(string? url) => new(url);

    public static ImageExpiresEvent Result(int resultCode) => new(resultCode);
}