using MessagePack;
using MessagePack.Formatters;
using System.Buffers;

namespace Lagrange.OneBot.Database;

public class StreamFormatter : IMessagePackFormatter<Stream?>
{
    public void Serialize(ref MessagePackWriter writer, Stream? value, MessagePackSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNil();
            return;
        }
        byte[] buffer = new byte[value.Length];
        value.ReadExactly(buffer);
        writer.Write(buffer);
    }

    public Stream? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil())
            return null;
        var sequence = reader.ReadBytes();
        if (sequence.HasValue)
        {
            byte[] buffer = sequence.Value.ToArray();
            return new MemoryStream(buffer);
        }
        return null;
    }
}
