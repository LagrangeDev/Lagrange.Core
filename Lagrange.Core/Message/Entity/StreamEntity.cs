using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

public class StreamEntity : IMessageEntity
{
    public string Text { get; }

    public StreamEntity(string text)
    {
        Text = text;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        byte[] extra;
        using (MemoryStream stream = new())
        {
            Serializer.Serialize(stream, new StreamExtra
            {
                Field43 = (ulong)Random.Shared.NextInt64(),
                Sequence = 1
            });
            extra = stream.ToArray();
        }

        return new Elem[] {
            new() {
                Text = new Text {
                    Str = Text
                }
            },
            new() {
                GeneralFlags = new GeneralFlags {
                    PbReserve = extra
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem) => throw new NotImplementedException();

    string IMessageEntity.ToPreviewString() => $"[Stream] {Text}";
}