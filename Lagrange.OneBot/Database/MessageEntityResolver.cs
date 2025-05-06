using Lagrange.Core.Message;
using MessagePack;
using MessagePack.Formatters;

namespace Lagrange.OneBot.Database;

public class MessageEntityResolver : IFormatterResolver
{
    private static readonly MessageEntityFormatter ENTITY_FORMATTER = new();

    private static readonly MessageChainFormatter CHAIN_FORMATTER = new();

    public IMessagePackFormatter<T>? GetFormatter<T>()
    {
        if (typeof(T) == typeof(IMessageEntity)) return (IMessagePackFormatter<T>)ENTITY_FORMATTER;

        if (typeof(T) == typeof(MessageChain)) return (IMessagePackFormatter<T>)CHAIN_FORMATTER;

        return null;
    }
}