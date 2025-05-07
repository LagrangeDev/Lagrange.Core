using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using MessagePack;
using MessagePack.Formatters;

namespace Lagrange.OneBot.Database;

public class MessageChainFormatter : IMessagePackFormatter<MessageChain?>
{
    public MessageChain? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return MessagePackSerializer.Deserialize<MessageRecord>(ref reader, options);
    }

    public void Serialize(ref MessagePackWriter writer, MessageChain? value, MessagePackSerializerOptions options)
    {
        if (value == null) MessagePackSerializer.Serialize(ref writer, null as MessageRecord, options);
        else MessagePackSerializer.Serialize(ref writer, (MessageRecord)value, options);
    }
}