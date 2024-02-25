using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Operation.Converters;

public static class SerializerOptions
{
    public static JsonSerializerOptions DefaultOptions => new()
    {
        Converters = { new BooleanConverter() },
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}
