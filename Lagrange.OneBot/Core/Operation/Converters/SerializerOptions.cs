using System.Text.Json;

namespace Lagrange.OneBot.Core.Operation.Converters;

public static class SerializerOptions
{
    public static JsonSerializerOptions DefaultOptions => new()
    {
        Converters = { new BooleanConverter(), new UInt32Converter(), new Int32Converter() },
    };
}
