using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.Action;

internal class ImageOcrEvent : ProtocolEvent
{
    public string Url { get; } = string.Empty;
    
    public ImageOcrResult ImageOcrResult { get; }
    
    private ImageOcrEvent(string url) : base(true) 
    {
        Url = url;
        ImageOcrResult = new ImageOcrResult(new List<TextDetection>(), "");
    }

    private ImageOcrEvent(int resultCode, ImageOcrResult result) : base(resultCode)
    {
        ImageOcrResult = result;
    }

    public static ImageOcrEvent Create(string url) => new(url);
    
    public static ImageOcrEvent Result(int resultCode, ImageOcrResult result) => new(resultCode, result);
}