using Lagrange.Core.Message;
using MessagePack;
using MessagePack.Formatters;

namespace Lagrange.OneBot.Database;

public class MessageEntityResolver : IFormatterResolver
{
    private static readonly MessageEntityFormatter _FORMATTER = new();

    private static readonly StreamFormatter _STREAM_FORMATTER = new();

    public IMessagePackFormatter<T>? GetFormatter<T>()
    {
        if (typeof(T) == typeof(IMessageEntity)) return (IMessagePackFormatter<T>?)_FORMATTER;
        if (typeof(T) == typeof(Stream)) return (IMessagePackFormatter<T>?)_STREAM_FORMATTER;
        return null;
    }
}