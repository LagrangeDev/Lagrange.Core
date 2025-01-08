using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Operation.Converters;

public class NullIfEmptyConverter<T> : JsonConverter<T> where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String when reader.ValueSpan.Length == 0 => null, 
            JsonTokenType.StartObject => JsonSerializer.Deserialize<T>(ref reader, options),
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, options);
}