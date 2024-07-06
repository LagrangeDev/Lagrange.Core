namespace Lagrange.Core.Internal.Event.Action;

internal class FetchMarketFaceKeyEvent : ProtocolEvent
{
    public List<string> FaceIds { get; } = new();

    public List<string>? Keys { get; } = new();

    private FetchMarketFaceKeyEvent(List<string> faceIds) : base(true)
    {
        FaceIds = faceIds;
    }

    protected FetchMarketFaceKeyEvent(int resultCode, List<string>? keys) : base(resultCode)
    {
        Keys = keys;
    }

    public static FetchMarketFaceKeyEvent Create(List<string> faceIds) => new(faceIds);

    public static FetchMarketFaceKeyEvent Result(int resultCode, List<string>? keys) => new(resultCode, keys);
}