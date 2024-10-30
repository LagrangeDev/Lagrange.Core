using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Operation.Converters;

public class AutosizeConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.String when Utf8Parser.TryParse(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan, out bool value, out _) => value,
            JsonTokenType.String when Utf8Parser.TryParse(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan, out int intValue, out _) => intValue != 0,
            JsonTokenType.Number when reader.TryGetInt32(out int val) => Convert.ToBoolean(val),
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteBooleanValue(value);
}
