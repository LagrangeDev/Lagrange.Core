using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Message.Entities;

public class TextEntity(string text) : IMessageEntity
{
    public string Text { get; } = text;

    public TextEntity() : this(string.Empty) { }
    
    Elem[] IMessageEntity.Build()
    {
        return
        [
            new Elem { Text = new Text { TextMsg = Text } }
        ];
    }
    
    string IMessageEntity.ToPreviewString() => Text;
    
    IMessageEntity? IMessageEntity.Parse(List<Elem> elements, Elem target)
    {
        // market face is followed by a text elem of its summary, which is already in MarketFaceEntity
        return target.Text is ({ Attr6Buf: null } or { Attr6Buf.Length: 0 }) and ({ PbReserve.Length: 0 }) && elements.All(x => x.MarketFace == null)
            ? new TextEntity(target.Text.TextMsg)
            : null;
    }
}