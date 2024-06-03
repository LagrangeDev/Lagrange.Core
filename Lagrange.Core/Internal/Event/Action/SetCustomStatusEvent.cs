namespace Lagrange.Core.Internal.Event.Action;

internal class SetCustomStatusEvent : SetStatusEvent
{
    public uint FaceId { get; }

    public string Text { get; } = string.Empty;

    private SetCustomStatusEvent(uint faceId, string text) : base(2000, 0)
    {
        FaceId = faceId;
        Text = text;
    }
    
    private SetCustomStatusEvent(int resultCode) : base(resultCode) { }
    
    public static SetCustomStatusEvent Create(uint faceId, string text) => new(faceId, text);

    public new static SetCustomStatusEvent Result(int resultCode) => new(resultCode);
}